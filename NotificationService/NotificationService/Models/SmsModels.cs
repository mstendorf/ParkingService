namespace NotificationService.Models
{
    public record SmsMessage(string receiver, string message);

    public record SmsRequest(string receiver, string message, string key);
}
