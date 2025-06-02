using App.Application.DTO;

namespace App.Application.Services
{
    public interface IBusLocationService
    {
        Task<BusLocationDTO> GetLatestBusLocationAsync(int tripId);
        Task UpdateBusLocationAsync(int driverId, BusLocationDTO dto);
        Task<double> GetBusAverageSpeedAsync(int tripId);
    }
}
