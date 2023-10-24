namespace ParkingServiceUnitTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using Moq;
    using CarTypeService.Services;
    using ParkingService.EventFeed;

    public class ParkingEndpoints_should : IDisposable
    {
        private readonly WebApplicationFactory<Program> _app;
        private readonly HttpClient sut;

        public ParkingEndpoints_should()
        {
            var carDesc = new CarTypeService.Models.CarDescription();
            carDesc.Make = "Tesla";
            carDesc.Model = "Model Y";
            carDesc.Variant = "Long Range";
            var mock = new Mock<IMotorApiService>();
            mock.SetReturnsDefault(carDesc);
            _app = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(
                    services => services.AddSingleton<IMotorApiService>(mock.Object)
                );
            });

            sut = _app.CreateClient();

            sut.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public void Dispose()
        {
            _app.Dispose();
            sut.Dispose();
        }

        [Fact]
        public void Return_no_content_when_parking_spot_empty()
        {
            var response = sut.GetAsync("/parking/check/dl20903/1a").Result;
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public void Return_ok_when_parking_spot_occupied()
        {
            // park car in spot 1a
            var requestData = new ParkingRequest("dl20903", "1a", "+45 27126901");
            var res = sut.PostAsJsonAsync<ParkingRequest>("/parking/register", requestData).Result;
            var response = sut.GetAsync("/parking/check/dl20903/1a").Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Returns_empty_response_after_deletion()
        {
            sut.DeleteAsync("/parking/dl20903");
            var response = sut.GetAsync("/parking/check/dl20903/1a").Result;
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        // this should be in its own class, but I am lazy atm, might refactor later
        [Fact]
        public void Return_list_of_one_event()
        {
            var response = sut.GetAsync("/events").Result;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var events = response.Content.ReadFromJsonAsync<Event[]>().Result;
            Assert.Single(events);
        }
    }

    public record ParkingRequest(
        string LicensePlate,
        string ParkingSpot,
        string ContactInformation
    );
}
