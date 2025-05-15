using App.Application.Services;

namespace App.Infrastructure.Services
{
    public class RouteScheduleService : IRouteScheduleService
    {

        public RouteScheduleService()
        {

        }

        /*public async Task AddRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules)
        {
            var schedules = routeSchedules
                .Select(rs => new RouteSchedule { RouteId = routeId }).ToList();
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
        }*/
    }
}
