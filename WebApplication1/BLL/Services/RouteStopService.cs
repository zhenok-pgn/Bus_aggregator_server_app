using App.DAL.EF;
using App.DAL.Entities;
using App.WEB.BLL.DTO;
using App.WEB.BLL.Interfaces;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace App.WEB.BLL.Services
{
    public class RouteStopService : IRouteStopService
    {
        public async Task AddRouteStops(int routeId, List<RouteStopDTO> routeStops)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var stops = routeStops.Select(rs => new RouteStop
            {
                RouteId = routeId,
                Order = rs.Order,
                BusStopId = rs.BusStopId,
                StopTimeInMinutes = rs.StopTimeInMinutes,
                MinutesFromStart = rs.MinutesFromStart,
                DistanceFromStart = rs.DistanceFromStart
            }).ToList();

            db.RouteStops.AddRange(stops);
            await db.SaveChangesAsync();
        }

        public async Task<RouteStopDTO> GetRouteStop(int id)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var routeStop = await db.RouteStops.FirstOrDefaultAsync(r => r.Id == id);
            return new RouteStopDTO
            {

            };
        }

        public async Task<List<RouteStopDTO>> GetRouteStops(int routeId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            return await db.RouteStops.Where(rs => rs.RouteId == routeId)
                .Select(rs => new RouteStopDTO
                {
                    Id = rs.Id,
                    Order = rs.Order,
                    BusStopId = rs.BusStopId,
                    StopTimeInMinutes = rs.StopTimeInMinutes,
                    MinutesFromStart = rs.MinutesFromStart,
                    DistanceFromStart = rs.DistanceFromStart
                }).ToListAsync();
        }

        public async Task<bool> RemoveRouteStops(int routeId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            db.RouteStops.RemoveRange(db.RouteStops.Where(rs => rs.RouteId == routeId));
            await db.SaveChangesAsync();

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
