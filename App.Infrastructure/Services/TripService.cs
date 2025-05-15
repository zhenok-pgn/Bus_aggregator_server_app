using App.Application.DTO;
using App.Application.Services;

namespace App.Infrastructure.Services
{
    public class TripService : ITripService
    {
        public TripService()
        {

        }
        public Task<List<TripDTO>> GetTripsAsync(int? carrierId, int? segmentId, DateTime? departureDateFrom, DateTime? departureDateTo, List<int> routes)
        {
            throw new NotImplementedException();
        }
        public Task<TripDTO> GetTripAsync(int? segmentId, int tripId)
        {
            throw new NotImplementedException();
        }
        public Task CreateAsync(int RouteScheduleId, DateTime fromPlanning, DateTime toPlanning, int carrierId, int busId, int driverId)
        {
            throw new NotImplementedException();
        }
        public Task UpdateAsync(TripDTO dto)
        {
            throw new NotImplementedException();
        }
        public Task<List<TripDTO>> GetDriversTripsAsync(int driverId)
        {
            throw new NotImplementedException();
        }
    }
}
