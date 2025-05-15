using App.Application.DTO;

namespace App.Application.Services
{
    public interface IRouteService
    {
        Task<List<RouteSummaryDTO>> GetRoutesAsync(int carrierId);
        Task<RouteDTO> GetRouteAsync(int carrierId, int routeId);
        Task CreateAsync(RouteDTO dto);
        Task UpdateAsync(RouteDTO dto);
        Task DeleteAsync(int carrierId, int routeId);
    }
}
