namespace ParkingService.Parking
{
    using Microsoft.AspNetCore.Mvc;
    using CarTypeService.Services;
    using EventFeed;

    [ApiController]
    [Route("[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly IparkingLot parkingLot;
        private readonly IMotorApiService motorApiService;
        private readonly IEventStore eventStore;

        public ParkingController(
            IparkingLot parkingLot,
            IMotorApiService motorApiService,
            IEventStore eventStore
        )
        {
            this.parkingLot = parkingLot;
            this.motorApiService = motorApiService;
            this.eventStore = eventStore;
        }

        [HttpPost("register")]
        public Registration Register(ParkingRequest request)
        {
            var carType = motorApiService.GetDescriptionAsync(request.LicensePlate);
            var registration = parkingLot.Register(request, carType.Result, eventStore);
            return registration;
        }

        [HttpGet("check/{licensePlate}/{parkingSpot}")]
        public ActionResult<Registration> CheckParking(string licensePlate, string parkingSpot)
        {
            var registration = parkingLot.CheckParking(licensePlate, parkingSpot);
            if (registration == null)
            {
                return NoContent();
            }
            return registration;
        }

        [HttpDelete("{licensePlate}")]
        public void Delete(string licensePlate)
        {
            parkingLot.Delete(licensePlate);
        }

        [HttpGet("registrations")]
        public List<Registration> GetRegistrations()
        {
            return parkingLot.GetRegistrations();
        }
    }
}
