using App.Application.DTO;

namespace App.Application.Interfaces.Services
{
    public interface IRouteScheduleService
    {
        Task AddRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules);
        Task<bool> RemoveRouteSchedules(int routeId);
        Task<List<RouteScheduleDTO>> GetRouteSchedules(int routeId);
        Task<bool> UpdateRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules);
    }
}
