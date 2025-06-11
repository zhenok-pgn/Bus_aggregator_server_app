using App.Application.DTO;

namespace App.Application.Services
{
    public interface IEtaCache
    {
        bool TryGet(int tripId, out EtaCacheEntry entry);
        void Set(int tripId, TripEtaDTO eta);
    }
}
