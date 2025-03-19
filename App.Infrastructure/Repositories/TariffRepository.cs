using App.Application.Interfaces.Repositories;
using App.Core.Entities;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Infrastructure.Repositories
{
    internal class TariffRepository : GenericRepository<Tariff>, ITariffRepository
    {
        private readonly ApplicationDBContext _context;
        public TariffRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Tariff> FirstOrDefaultAsync(Expression<Func<Tariff, bool>> predicate)
        {
            return await _context.Set<Tariff>()
                .Include(t => t.Prices)
                .FirstOrDefaultAsync(predicate);
        }

        public override async Task<IEnumerable<Tariff>> FindAsync(Expression<Func<Tariff, bool>> predicate)
        {
            return await _context.Set<Tariff>()
                .Include(t => t.Prices)
                .Where(predicate)
                .ToListAsync();
        }
    }
}
