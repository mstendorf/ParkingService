namespace NotificationService.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class NotifyService : INotifyService
    {
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;

        public NotifyService(ISmsService smsService, IEmailService emailService)
        {
            _smsService = smsService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Notify(NotifyRequest request)
        {
            if (request.receiver.Contains("@"))
            {
                var emailMessage = new EmailMessage(
                    request.receiver,
                    request.subject,
                    request.message,
                    request.html
                );
                var response = await _emailService.SendEmailAsync(emailMessage);
                if (response is StatusCodeResult)
                {
                    return new OkResult();
                }
                else
                {
                    return new BadRequestResult();
                }
            }
            else
            {
                var message = new SmsMessage(request.receiver, request.message);
                var response = await _smsService.SendSmsAsync(message);
                if (response is StatusCodeResult)
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
}
