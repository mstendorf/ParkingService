// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

Console.WriteLine("Hello, World!");
var client = new HttpClient();
long start = await GetLastSentMessageId();
long end = long.MaxValue;
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
using var response = await client.GetAsync(
    $"http://parking-service/events?start={start}&end={end}"
);
Console.WriteLine($"gonna process events from {start} to {end}");
await ProcessEvents(await response.Content.ReadAsStreamAsync());

async Task<long> GetLastSentMessageId()
{
    long lastSentMessageId = (long)
        Convert.ToDouble(
            await client.GetStringAsync("http://notification-service:8080/notification")
        );
    return lastSentMessageId + 1;
}
async Task ProcessEvents(Stream rawEvents)
{
    var events = await JsonSerializer.DeserializeAsync<Event[]>(rawEvents);
    Console.WriteLine(events);
    foreach (var @event in events)
    {
        Console.WriteLine(@event);
        var request = new NotifyRequest(
            @event.sequenceNumber,
            @event.content.contactInformation,
            "Parkering startet",
            $"Parkering startet for {@event.content.carType.make} {@event.content.carType.model} med nummerpladen {@event.content.licensePlate} på plads {@event.content.parkingSpot}",
            $"Parkering startet for <b>{@event.content.carType.make} {@event.content.carType.model}</b> med nummerpladen <b>{@event.content.licensePlate}</b> på plads <b>{@event.content.parkingSpot}</b>"
        );
        // use httpclient to send request to notify service
        var response = await client.PostAsJsonAsync<NotifyRequest>(
            "http://notification-service:8080/notification",
            request
        );
        Console.WriteLine(response);
    }
}

public record Event(long sequenceNumber, DateTimeOffset occurredAt, string name, Parking content);

public record CarInfo(string make, string model, string variant);

public record Parking(
    string licensePlate,
    string contactInformation,
    string parkingSpot,
    DateTimeOffset parkedTime,
    CarInfo carType
);

public record NotifyRequest(
    long sequenceId,
    string receiver,
    string? subject,
    string message,
    string? html
);
