namespace NotificationService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotifyService _notifyService;
        private static IList<long> _messageIds = new List<long>();

        public NotificationController(INotifyService notifyService)
        {
            _notifyService = notifyService;
        }

        [HttpPost]
        public async Task<IActionResult> Notify([FromBody] NotifyRequest request)
        {
            Console.WriteLine($"Notify: {request.sequenceId}");
            _messageIds.Add(request.sequenceId);
            var response = await _notifyService.Notify(request);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult LastSentNotificationId()
        {
            Console.WriteLine($"LastSentNotificationId: {_messageIds.LastOrDefault()}");
            var result = _messageIds.LastOrDefault();
            return Ok(result);
        }
    }
}
