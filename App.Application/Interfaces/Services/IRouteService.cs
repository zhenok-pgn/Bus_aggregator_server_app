using App.Application.DTO;

namespace App.Application.Interfaces.Services
{
    internal interface IRouteService
    {
        Task AddRoute(RouteDTO route);
        Task<bool> RemoveRoute(int routeId);
        Task<RouteDTO> GetRouteById(int routeId);
        Task<bool> UpdateRoute(RouteDTO route);
    }
}
