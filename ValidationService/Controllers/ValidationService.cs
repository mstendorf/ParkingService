namespace ValidationService
{
    using Microsoft.AspNetCore.Mvc;
    using PlateRecognizer;
    using System.IO;

    [Route("validate")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private static PlateReaderClient PlateReader = new PlateReaderClient();
        private readonly IParkingGarageClient ParkingGarageClient;

        public ValidationController(IParkingGarageClient parkingGarageClient)
        {
            ParkingGarageClient = parkingGarageClient;
        }

        // recieve an image for validation
        [HttpPost("image")]
        public async Task<IActionResult> Post(IFormFile file, string parkingSpot)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was uploaded");

            if (!file.ContentType.Contains("image"))
                return BadRequest("The file is not an image");

            if (file.Length > 10000000)
                return BadRequest("The file is too large");

            byte[] image;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                image = ms.ToArray();
            }

            var plate = PlateReader.GetPlateFromImage(image);
            if (plate == null)
                return BadRequest("No license plate was found in the image");

            var registration = await ParkingGarageClient.CheckParking(plate, parkingSpot);
            Console.WriteLine(registration);
            if (registration == null)
                return BadRequest("The license plate is not registered");
            if (!validateParking(registration))
                return BadRequest("The license plate is not registered today");
            return Ok(registration);
        }

        private bool validateParking(Registration registration)
        {
            var parkedToday = registration.ParkedTime.Date == DateTime.Now.Date;
            return parkedToday;
        }
    }
}
