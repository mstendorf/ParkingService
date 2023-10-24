using CarTypeService.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace CarTypeService.Services
{
    public class MotorApiService : IMotorApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MotorApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<CarDescription> GetDescriptionAsync(string licensePlate)
        {
            string url = $"https://v1.motorapi.dk/vehicles/{licensePlate}";
            string key = _configuration.GetValue<string>("MotorApiKey");
            Console.WriteLine($"MotorApiKey: {key}");
            Console.WriteLine($"url: {url}");
            using (var c = _httpClientFactory.CreateClient())
            {
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                c.DefaultRequestHeaders.Add("x-auth-token", key);
                var result = await c.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    var content = result.Content.ReadAsStringAsync().Result;
                    CarType? car = JsonConvert.DeserializeObject<CarType>(content);

                    if (car != null)
                    {
                        return new()
                        {
                            Make = car.make,
                            Model = car.model,
                            Variant = car.variant
                        };
                    }
                    return CarDescription.NONE;
                }
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return CarDescription.NONE;
                }
                else
                {
                    throw new MotorApiException(result.StatusCode, result.Content);
                }
            }
        }
    }
}
