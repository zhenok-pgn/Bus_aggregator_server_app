using App.Application.Interfaces.Repositories;
using App.Core.Entities;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Repositories
{
    public class RouteStopRepository : GenericRepository<RouteStop>, IRouteStopRepository
    {
        public RouteStopRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
