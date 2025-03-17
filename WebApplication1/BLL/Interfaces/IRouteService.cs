using App.BLL.DTO;

namespace App.BLL.Interfaces
{
    internal interface IRouteService
    {
        Task AddRoute(RouteDTO route);
        Task<bool> RemoveRoute(int routeId);
        Task<RouteDTO> GetRouteById(int routeId);
        Task<bool> UpdateRoute(RouteDTO route);
    }
}
