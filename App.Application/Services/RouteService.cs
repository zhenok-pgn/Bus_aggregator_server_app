using App.Application.DTO;
using App.Application.Interfaces;
using App.Application.Interfaces.Services;
using App.Core.Entities;

namespace App.Application.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteStopService _routeStopService;
        private readonly ITariffService _tariffService;
        private readonly IRouteScheduleService _routeScheduleService;
        private readonly IUnitOfWork _unitOfWork;

        public RouteService(IRouteStopService routeStopService, ITariffService tariffService, IRouteScheduleService routeScheduleService, IUnitOfWork unitOfWork)
        {
            _routeStopService = routeStopService;
            _tariffService = tariffService;
            _routeScheduleService = routeScheduleService;
            _unitOfWork = unitOfWork;
        }

        public async Task AddRoute(RouteDTO route)
        {
            var routeDb = new Route
            {
                Name = route.Name,
                Number = route.Number,
                CarrierId = route.Carrier.Id
            };

            await _unitOfWork.Routes.AddAsync(routeDb);
            await _unitOfWork.SaveChangesAsync();

            await _routeStopService.AddRouteStops(route.Id, route.RouteStops);
            await _tariffService.AddTariffs(route.Id, route.Tariffs);
            await _routeScheduleService.AddRouteSchedules(route.Id, route.RouteSchedules);
        }

        public async Task<RouteDTO?> GetRouteById(int routeId)
        {
            var route = await _unitOfWork.Routes.GetByIdWithCarrierAsync(routeId);

            if (route == null) return null;

            var routeStops = await _routeStopService.GetRouteStops(routeId);
            var tariffs = await _tariffService.GetTariffs(routeId);
            var routeSchedules = await _routeScheduleService.GetRouteSchedules(routeId);

            return new RouteDTO
            {
                Id = route.Id,
                Name = route.Name,
                Number = route.Number,
                Carrier = new(),
                RouteStops = routeStops,
                Tariffs = tariffs,
                RouteSchedules = routeSchedules
            };
        }

        public async Task<bool> RemoveRoute(int routeId)
        {
            var route = await _unitOfWork.Routes.GetByIdAsync(routeId);
            if (route == null) return false;

            await _routeStopService.RemoveRouteStops(routeId);
            await _tariffService.RemoveTariffs(routeId);
            await _routeScheduleService.RemoveRouteSchedules(routeId);

            await _unitOfWork.Routes.RemoveAsync(route.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoute(RouteDTO route)
        {
            var existingRoute = await _unitOfWork.Routes.GetByIdAsync(route.Id);

            if (existingRoute == null) return false;

            existingRoute.Name = route.Name;
            existingRoute.Number = route.Number;
            existingRoute.CarrierId = route.Carrier.Id;

            await _unitOfWork.SaveChangesAsync();

            await _routeStopService.UpdateRouteStops(route.Id, route.RouteStops);
            await _tariffService.UpdateTariffs(route.Id, route.Tariffs);
            await _routeScheduleService.UpdateRouteSchedules(route.Id, route.RouteSchedules);

            return true;
        }
    }
}
