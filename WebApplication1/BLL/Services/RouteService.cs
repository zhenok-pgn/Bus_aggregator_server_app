using App.BLL.DTO;
using App.BLL.Interfaces;
using App.DAL.EF;
using App.DAL.Entities;
using App.WEB.BLL.DTO;
using App.WEB.BLL.Infrastructure;
using App.WEB.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.WEB.BLL.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteStopService _routeStopService;
        private readonly ITariffService _tariffService;
        private readonly IRouteScheduleService _routeScheduleService;

        public RouteService(IRouteStopService routeStopService, ITariffService tariffService, IRouteScheduleService routeScheduleService)
        {
            _routeStopService = routeStopService;
            _tariffService = tariffService;
            _routeScheduleService = routeScheduleService;
        }

        public async Task AddRoute(RouteDTO route)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var routeDb = new DAL.Entities.Route
            {
                Name = route.Name,
                Number = route.Number,
                CarrierId = route.Carrier.Id
            };

            db.Routes.Add(routeDb);
            await db.SaveChangesAsync();

            await _routeStopService.AddRouteStops(route.Id, route.RouteStops);
            await _tariffService.AddTariffs(route.Id, route.Tariffs);
            await _routeScheduleService.AddRouteSchedules(route.Id, route.RouteSchedules);
        }

        public async Task<RouteDTO?> GetRouteById(int routeId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();

            var route = await db.Routes
                .Include(r => r.Carrier)
                .FirstOrDefaultAsync(r => r.Id == routeId);

            if (route == null) return null;

            var routeStops = await _routeStopService.GetRouteStops(routeId);
            var tariffs = await _tariffService.GetTariffs(routeId);
            var routeSchedules = await _routeScheduleService.GetRouteSchedules(routeId);

            return new RouteDTO
            {
                Id = route.Id,
                Name = route.Name,
                Number = route.Number,
                Carrier = route.Carrier.MapToDto<CarrierDTO>(),
                RouteStops = routeStops,
                Tariffs = tariffs,
                RouteSchedules = routeSchedules
            };
        }

        public async Task<bool> RemoveRoute(int routeId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();

            var route = await db.Routes.FindAsync(routeId);
            if (route == null) return false;

            await _routeStopService.RemoveRouteStops(routeId);
            await _tariffService.RemoveTariffs(routeId);
            await _routeScheduleService.RemoveRouteSchedules(routeId);

            db.Routes.Remove(route);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoute(RouteDTO route)
        {
            using ApplicationDBContext db = new ApplicationDBContext();

            var existingRoute = await db.Routes.FirstOrDefaultAsync(r => r.Id == route.Id);

            if (existingRoute == null) return false;

            existingRoute.Name = route.Name;
            existingRoute.Number = route.Number;
            existingRoute.CarrierId = route.Carrier.Id;

            await db.SaveChangesAsync();

            await _routeStopService.UpdateRouteStops(route.Id, route.RouteStops);
            await _tariffService.UpdateTariffs(route.Id, route.Tariffs);
            await _routeScheduleService.UpdateRouteSchedules(route.Id, route.RouteSchedules);

            return true;
        }
    }
}
