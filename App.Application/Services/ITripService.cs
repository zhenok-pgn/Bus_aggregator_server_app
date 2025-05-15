using App.Application.DTO;
using App.Core.Entities;

namespace App.Application.Services
{
    public interface ITripService
    {
        Task<List<TripDTO>> GetTripsAsync(int? carrierId, int? segmentId, DateTime? departureDateFrom, DateTime? departureDateTo, List<int> routes);
        Task<TripDTO> GetTripAsync(int? segmentId, int tripId);
        Task CreateAsync(int RouteScheduleId, DateTime fromPlanning, DateTime toPlanning, int carrierId, int busId, int driverId);
        Task UpdateAsync(TripDTO dto);
        Task<List<TripDTO>> GetDriversTripsAsync(int driverId);
    }
}
