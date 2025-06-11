using App.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace App.Infrastructure.Hubs
{
    [Authorize]
    public class TripHub : Hub
    {
        private readonly IEtaService _etaService;
        private readonly IBusLocationService _busLocationService;

        public TripHub(IEtaService etaService, IBusLocationService busLocationService)
        {
            _etaService = etaService;
            _busLocationService = busLocationService;
        }

        public async Task JoinTripGroup(string groupId)
        {
            var parts = groupId.Split('_');
            await Groups.AddToGroupAsync(Context.ConnectionId, parts[1]);
            var eta = await _etaService.CalculateEtaAsync(int.Parse(parts[0]));
            var latestLocation = await _busLocationService.GetLatestBusLocationAsync(int.Parse(parts[0]));
            if(latestLocation != null)
            {
                await Clients.Caller.SendAsync("TripLocationUpdated", latestLocation);
            }
            if (eta != null)
            {
                await Clients.Caller.SendAsync("EtaUpdated", eta);
            }
        }

        public async Task LeaveTripGroup(string routeSegmentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, routeSegmentId);
        }
    }
}
