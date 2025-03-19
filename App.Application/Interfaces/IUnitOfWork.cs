using App.Application.Interfaces.Repositories;
using App.Core.Entities;

namespace App.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IRouteRepository Routes { get; } 
        IRouteScheduleRepository RouteSchedules { get; } 
        IRouteStopRepository RouteStops { get; }
        ITariffRepository Tariffs { get; }

        Task<int> SaveChangesAsync(); // 👈 Одна точка сохранения изменений
    }
}
