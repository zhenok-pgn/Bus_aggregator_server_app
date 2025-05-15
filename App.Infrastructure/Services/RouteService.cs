using App.Application.DTO;
using App.Application.Services;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Services
{
    public class RouteService : IRouteService
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public RouteService(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task CreateAsync(RouteDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int carrierId, int routeId)
        {
            throw new NotImplementedException();
        }

        public Task<RouteDTO> GetRouteAsync(int carrierId, int routeId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RouteSummaryDTO>> GetRoutesAsync(int carrierId)
        {
            var routes = await _db.Routes
            .Where(r => r.CarrierId == carrierId)
            .ToListAsync();

            return _mapper.Map<List<RouteSummaryDTO>>(routes);
        }

        public Task UpdateAsync(RouteDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
