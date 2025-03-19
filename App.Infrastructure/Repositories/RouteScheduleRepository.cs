using App.Application.Interfaces.Repositories;
using App.Core.Entities;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Repositories
{
    public class RouteScheduleRepository : GenericRepository<RouteSchedule>, IRouteScheduleRepository
    {

        public RouteScheduleRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
