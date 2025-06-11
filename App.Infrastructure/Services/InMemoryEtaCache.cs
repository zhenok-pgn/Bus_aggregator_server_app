using App.Application.DTO;
using App.Application.Services;
using System.Collections.Concurrent;

namespace App.Infrastructure.Services
{
    public class InMemoryEtaCache : IEtaCache
    {
        private readonly ConcurrentDictionary<int, EtaCacheEntry> _cache = new();

        public bool TryGet(int tripId, out EtaCacheEntry entry) => _cache.TryGetValue(tripId, out entry);

        public void Set(int tripId, TripEtaDTO eta)
        {
            _cache[tripId] = new EtaCacheEntry
            {
                LastUpdatedUtc = DateTimeOffset.UtcNow,
                TripEta = eta
            };
        }
    }
}
