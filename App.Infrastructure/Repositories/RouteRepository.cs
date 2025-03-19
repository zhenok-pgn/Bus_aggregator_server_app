using App.Application.Interfaces.Repositories;
using App.Core.Entities;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Repositories
{
    public class RouteRepository : GenericRepository<Route>, IRouteRepository
    {
        private readonly ApplicationDBContext _context;

        public RouteRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Route> GetByIdWithCarrierAsync(int id)
        {
            return await _context.Set<Route>().Include(r => r.Carrier).FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
