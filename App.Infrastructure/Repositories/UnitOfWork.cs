using App.Application.Interfaces;
using App.Application.Interfaces.Repositories;
using App.Infrastructure.Data;

namespace App.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;

        public UnitOfWork(ApplicationDBContext context,
                         IUserRepository userRepository,
                         IRefreshTokenRepository refreshTokenRepository,
                         IRouteRepository routeRepository,
                         IRouteScheduleRepository routeScheduleRepository,
                         IRouteStopRepository routeStopRepository,
                         ITariffRepository tariffRepository)
        {
            _context = context;
            Users = userRepository;
            RefreshTokens = refreshTokenRepository;
            Routes = routeRepository;
            RouteSchedules = routeScheduleRepository;
            RouteStops = routeStopRepository;
            Tariffs = tariffRepository;
        }

        public IUserRepository Users { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        public IRouteRepository Routes { get; }
        public IRouteScheduleRepository RouteSchedules { get; }
        public IRouteStopRepository RouteStops { get; }
        public ITariffRepository Tariffs { get; }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
