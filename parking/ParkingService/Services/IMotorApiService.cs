using CarTypeService.Models;

namespace CarTypeService.Services
{
    public interface IMotorApiService
    {
        Task<CarDescription> GetDescriptionAsync(string licensePlate);
    }
}