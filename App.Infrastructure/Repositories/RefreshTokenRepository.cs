using App.Application.Interfaces.Repositories;
using App.Core.Entities;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDBContext _context;

        public RefreshTokenRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken> FirstOrDefaultWithUserAsync(Expression<Func<RefreshToken, bool>> predicate)
        {
            return await _context.Set<RefreshToken>().Include(r => r.User).FirstOrDefaultAsync(predicate);
        }
    }
}
