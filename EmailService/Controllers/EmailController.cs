using EmailService.Services;
using Microsoft.AspNetCore.Mvc;
using EmailService.Models;

namespace EmailService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailMessage emailMessage)
        {
            // send email and validate we got a http 200 response
            var response = await _emailService.SendEmailAsync(emailMessage);
            if (response is StatusCodeResult)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
