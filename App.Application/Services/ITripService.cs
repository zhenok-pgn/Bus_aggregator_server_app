using App.Application.DTO;
using App.Application.DTO.Requests;

namespace App.Application.Services
{
    public interface ITripService
    {
        Task<List<TripDTO>> GetTripsAsync(int? carrierId, int? localityFromId, int? localityToId, DateOnly? departureDateFrom, DateOnly? departureDateTo, List<int> routes);
        Task<TripDTO> GetTripAsync(int? segmentId, int tripId);
        Task CreateAsync(TripPlanRequest planRequest, int carrierId);
        Task UpdateAsync(TripDTO dto);
        Task<List<TripDTO>> GetDriversTripsAsync(int driverId);
        Task<TripExecutionDTO> GetTripExecutionAsync(int tripId);
        Task ConfirmArrivalAsync(int driverId, int tripId, int segmentId, DateTimeOffset timestamp);
        Task ConfirmDepartureAsync(int driverId, int tripId, int segmentId, DateTimeOffset timestamp);
    }
}
