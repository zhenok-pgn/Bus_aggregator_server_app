using App.Application.DTO;
using App.Application.Services;
using App.Infrastructure.Data;
using App.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Services
{
    public class TripNotifier : ITripNotifier
    {
        private readonly IHubContext<TripHub> _hubContext;
        private readonly ApplicationDBContext _db;

        public TripNotifier(IHubContext<TripHub> hubContext, ApplicationDBContext db)
        {
            _hubContext = hubContext;
            _db = db;
        }

        public async Task SendLocationUpdateAsync(int tripId, BusLocationDTO location)
        {
            var routeScheduleId = await _db.Trips
                .Where(t => t.Id == tripId)
                .Select(t => t.RouteScheduleId)
                .FirstOrDefaultAsync();
            var routeSegmentIds = await _db.RouteSegmentSchedules
                .Where(rss => rss.RouteScheduleId == routeScheduleId)
                .Select(rss => rss.RouteSegmentId.ToString())
                .ToListAsync();
            foreach(var i in routeSegmentIds)
            {
                await _hubContext.Clients.Group(i)
                    .SendAsync("TripLocationUpdated", location);
            }
        }

        public Task SendStatusUpdateAsync(int routeSegmentId, string status)
        {
            return _hubContext.Clients.Group(routeSegmentId.ToString())
                .SendAsync("TripStatusChanged", status);
        }

        public Task SendEtaUpdateAsync(int routeSegmentId, TripEtaDTO eta)
        {
            return _hubContext.Clients.Group(routeSegmentId.ToString())
                .SendAsync("EtaUpdated", eta);
        }
    }
}
