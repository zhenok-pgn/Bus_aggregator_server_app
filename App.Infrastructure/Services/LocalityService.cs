using App.Application.DTO;
using App.Application.Services;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Services
{
    public class LocalityService : ILocalityService
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        public LocalityService(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<LocalityDTO>> GetLocalitiesAsync()
        {
            var localities = await _db.Localities.Include(l=>l.UtcTimezone).ToListAsync();
            return _mapper.Map<List<LocalityDTO>>(localities);
        }

        public async Task<LocalityDTO> GetLocalityAsync(int localityId)
        {
            var locality = await _db.Localities.Where(l=>l.OsmId == localityId).Include(l => l.UtcTimezone).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException($"Locality with id {localityId} not found.");
            return _mapper.Map<LocalityDTO>(locality);
        }
    }
}
