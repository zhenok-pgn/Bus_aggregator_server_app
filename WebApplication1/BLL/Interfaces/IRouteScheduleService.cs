using App.BLL.DTO;
using App.WEB.BLL.DTO;

namespace App.WEB.BLL.Interfaces
{
    public interface IRouteScheduleService
    {
        Task AddRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules);
        Task<bool> RemoveRouteSchedules(int routeId);
        Task<List<RouteScheduleDTO>> GetRouteSchedules(int routeId);
        Task<bool> UpdateRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules);
    }
}
