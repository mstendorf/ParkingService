namespace NotificationService.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public interface IEmailService
    {
        Task<IActionResult> SendEmailAsync(EmailMessage emailMessage);
    }
}
