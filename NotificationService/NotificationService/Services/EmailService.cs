namespace NotificationService.Services
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class MailService : IEmailService
    {
        private readonly HttpClient client;

        private static readonly string emailApiUrl =
            "https://twilioproxy.azurewebsites.net/api/SendEmail?code=qMTJzZtnKGD4c0LgyYHyepoT7VdFOir1Wig9yrU6LeQLAzFuCJeiWw==";

        // private static readonly string emailApiUrl =
        //     "https://localhost:7071/api/SendEmail?code=qMTJzZtnKGD4c0LgyYHyepoT7VdFOir1Wig9yrU6LeQLAzFuCJeiWw==";

        public MailService(HttpClient httpClient)
        {
            client = httpClient;
        }

        public async Task<IActionResult> SendEmailAsync(EmailMessage emailMessage)
        {
            var response = await client.PostAsJsonAsync(emailApiUrl, emailMessage);
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
