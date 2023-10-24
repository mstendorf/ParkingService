namespace NotificationService.Services
{
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net.Http.Json;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class TextService : ISmsService
    {
        private readonly HttpClient httpClient;

        // bad practice, refactor this to use secrets, but pretty sure this service will be carved up in the future.
        private static readonly string key = "B0D7981D";
        private static readonly string smsApiUrl =
            "https://twilioproxy.azurewebsites.net/api/SendSMS?code=biIj0VMV608PIppCMrQDNn477AqqA7-w4a7mE8kug2HvAzFuxgovmQ==";

        public TextService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> SendSmsAsync(SmsMessage request)
        {
            var smsRequest = new SmsRequest(request.receiver, request.message, key);
            Console.WriteLine("Sending SMS...2");
            var response = await httpClient.PostAsJsonAsync(smsApiUrl, smsRequest);
            Console.WriteLine("Response:");
            Console.WriteLine(response.ToString());
            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
