using App.Core.Entities;
using System.Linq.Expressions;

namespace App.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> FirstOrDefaultWithUserAsync(Expression<Func<RefreshToken, bool>> predicate);
    }
}
