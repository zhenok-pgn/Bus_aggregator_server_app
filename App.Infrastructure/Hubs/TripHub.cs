using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace App.Infrastructure.Hubs
{
    [Authorize]
    public class TripHub : Hub
    {
        public async Task JoinTripGroup(string routeSegmentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, routeSegmentId);
        }

        public async Task LeaveTripGroup(string routeSegmentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, routeSegmentId);
        }
    }
}
