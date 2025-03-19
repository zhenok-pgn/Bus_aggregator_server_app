using App.Application.DTO;
using App.Application.Interfaces;
using App.Application.Interfaces.Services;
using App.Core.Entities;

namespace App.Application.Services
{
    public class RouteStopService : IRouteStopService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RouteStopService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddRouteStops(int routeId, List<RouteStopDTO> routeStops)
        {
            var stops = routeStops.Select(rs => new RouteStop
            {
                RouteId = routeId,
                Order = rs.Order,
                BusStopId = rs.BusStopId,
                StopTimeInMinutes = rs.StopTimeInMinutes,
                MinutesFromStart = rs.MinutesFromStart,
                DistanceFromStart = rs.DistanceFromStart
            }).ToList();

            await _unitOfWork.RouteStops.AddRangeAsync(stops);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<RouteStopDTO> GetRouteStop(int id)
        {
            var routeStop = await _unitOfWork.RouteStops.FirstOrDefaultAsync(r => r.Id == id);
            return new RouteStopDTO
            {

            };
        }

        public async Task<List<RouteStopDTO>> GetRouteStops(int routeId)
        {
            var routeStops = await _unitOfWork.RouteStops.FindAsync(rs => rs.RouteId == routeId);
            return routeStops
                .Select(rs => new RouteStopDTO
                {
                    Id = rs.Id,
                    Order = rs.Order,
                    BusStopId = rs.BusStopId,
                    StopTimeInMinutes = rs.StopTimeInMinutes,
                    MinutesFromStart = rs.MinutesFromStart,
                    DistanceFromStart = rs.DistanceFromStart
                }).ToList();
        }

        public async Task<bool> RemoveRouteStops(int routeId)
        {
            await _unitOfWork.RouteStops.RemoveRange(await _unitOfWork.RouteStops.FindAsync(rs => rs.RouteId == routeId));
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRouteStops(int routeId, List<RouteStopDTO> routeStops)
        {
            await RemoveRouteStops(routeId);
            await AddRouteStops(routeId, routeStops);

            return true;
        }
    }
}
