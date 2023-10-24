namespace ParkingService.EventFeed
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    [Route("/events")]
    public class EventFeedController
    {
        private readonly IEventStore eventStore;

        public EventFeedController(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        [HttpGet]
        public Event[] Get([FromQuery] long start, [FromQuery] long end = long.MaxValue)
        {
            Console.WriteLine($"Get events from {start} to {end}");
            return this.eventStore.GetEvents(start, end).ToArray();
        }
    }
}
