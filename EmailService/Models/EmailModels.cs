namespace EmailService.Models
{
    public record EmailMessage(string receiver, string subject, string message, string html);
}
