using App.Application.DTO;

namespace App.Application.Services
{
    public interface ITripNotifier
    {
        Task SendLocationUpdateAsync(int tripId, BusLocationDTO location);
        Task SendStatusUpdateAsync(int routeSegmentId, string status);
        Task SendEtaUpdateAsync(int routeSegmentId, TripEtaDTO eta);
    }
}
