namespace ValidationService
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Net.Http.Headers;

    public interface IParkingGarageClient
    {
        Task<Registration?> CheckParking(string Plate, string ParkingSpot);
    }

    public class ParkingGarageClient : IParkingGarageClient
    {
        private static string getUrl = "http://validation-service:9000/parking/check/";
        private readonly HttpClient client;

        public ParkingGarageClient(HttpClient client)
        {
            this.client = client;
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<Registration?> CheckParking(string Plate, string ParkingSpot)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                getUrl + Plate + "/" + ParkingSpot
            );

            var response = await client.SendAsync(request);
            Console.WriteLine($"{response.StatusCode}, {response.IsSuccessStatusCode}");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var registration = await JsonSerializer.DeserializeAsync<Registration>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return registration;
        }
    }

    public record Registration(
        string LicensePlate,
        string ContactInformation,
        string ParkingSpot,
        DateTime ParkedTime,
        CarDescription CarType
    );

    public record CarDescription(string Make, string Model, string Variant);
}
