namespace NotificationService.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public interface ISmsService
    {
        Task<IActionResult> SendSmsAsync(SmsMessage request);
    }
}
