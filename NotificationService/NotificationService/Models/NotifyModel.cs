namespace NotificationService.Models
{
    public record NotifyRequest(
        long sequenceId,
        string receiver,
        string? subject,
        string message,
        string? html
    );
}
