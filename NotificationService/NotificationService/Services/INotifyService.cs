namespace NotificationService.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public interface INotifyService
    {
        Task<IActionResult> Notify(NotifyRequest request);
    }
}
