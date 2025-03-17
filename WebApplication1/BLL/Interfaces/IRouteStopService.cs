using App.BLL.DTO;
using App.WEB.BLL.DTO;

namespace App.WEB.BLL.Interfaces
{
    public interface IRouteStopService
    {
        Task AddRouteStops(int routeId, List<RouteStopDTO> routeStops);
        Task<bool> RemoveRouteStops(int routeId);
        Task<List<RouteStopDTO>> GetRouteStops(int routeId);
        Task<RouteStopDTO> GetRouteStop(int id);
        Task<bool> UpdateRouteStops(int routeId, List<RouteStopDTO> routeStops);
    }
}
