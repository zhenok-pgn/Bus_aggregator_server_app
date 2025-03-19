using App.Core.Entities;

namespace App.Application.Interfaces.Repositories
{
    public interface IRouteRepository : IGenericRepository<Route>
    {
        Task<Route> GetByIdWithCarrierAsync(int id);
    }
}
