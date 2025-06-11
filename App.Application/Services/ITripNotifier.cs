using App.Application.DTO;

namespace App.Application.Services
{
    public interface ITripNotifier
    {
        Task SendLocationUpdateAsync(int tripId, BusLocationDTO location);
        Task SendRouteSegmentStatusUpdateAsync(int routeSegmentId, string status);
        Task SendTripStatusUpdateAsync(int tripId, string status);
        Task SendEtaUpdateAsync(int tripId, TripEtaDTO eta);
    }
}
