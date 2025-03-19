using App.Application.DTO;
using App.Application.Interfaces;
using App.Application.Interfaces.Services;
using App.Core.Entities;

namespace App.Application.Services
{
    public class RouteScheduleService : IRouteScheduleService
    {
        private readonly ITariffService _tariffService;
        private readonly IUnitOfWork _unitOfWork;

        public RouteScheduleService(ITariffService tariffService, IUnitOfWork unitOfWork)
        {
            _tariffService = tariffService;
            _unitOfWork = unitOfWork;
        }

        public async Task AddRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules)
        {
            var schedules = routeSchedules
                .Select(rs => new RouteSchedule { RouteId = routeId, BaseSeatingPlan = rs.BaseSeatingPlan }).ToList();
            await _unitOfWork.RouteSchedules.AddRangeAsync(schedules);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<RouteScheduleDTO>> GetRouteSchedules(int routeId)
        {
            var tariffs = await _tariffService.GetTariffs(routeId);
            var schedules = await _unitOfWork.RouteSchedules
                .FindAsync(rs => rs.RouteId == routeId);
            return schedules
                .Select(rs => new RouteScheduleDTO { BaseSeatingPlan = rs.BaseSeatingPlan, Tariff = tariffs.FirstOrDefault(t => t.Id == rs.TariffId) }).ToList();
        }

        public async Task<bool> RemoveRouteSchedules(int routeId)
        {
            await _unitOfWork.RouteSchedules.RemoveRange(await _unitOfWork.RouteSchedules.GetAllAsync());
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules)
        {
            await RemoveRouteSchedules(routeId);
            await AddRouteSchedules(routeId, routeSchedules);

            return true;
        }
    }
}
