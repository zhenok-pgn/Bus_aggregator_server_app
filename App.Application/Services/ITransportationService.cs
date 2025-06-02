using App.Application.DTO;
using App.Application.DTO.Requests;

namespace App.Application.Services
{
    public interface ITransportationService
    {
        Task<List<DriverDTO>> GetDriversAsync(int carrierId);
        Task<DriverDTO> GetDriverAsync(int carrierId, int driverId);
        Task CreateDriverAsync(DriverRegisterRequest request);
        Task UpdateDriverAsync(DriverDTO dto);
        Task DeleteDriverAsync(int carrierId, int routeId);
        Task<List<BusDTO>> GetBusesAsync(int carrierId);
        Task<BusDTO> GetBusAsync(int carrierId, int busId);
        Task CreateBusAsync(BusDTO dto);
        Task UpdateBusAsync(BusDTO dto);
        Task DeleteBusAsync(int carrierId, int busId);
    }
}
