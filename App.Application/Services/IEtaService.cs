using App.Application.DTO;

namespace App.Application.Services
{
    public interface IEtaService
    {
        Task<TripEtaDTO> CalculateEtaAsync(int tripId);
    }
}
