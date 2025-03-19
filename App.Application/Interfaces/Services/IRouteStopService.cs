using App.Application.DTO;

namespace App.Application.Interfaces.Services
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
