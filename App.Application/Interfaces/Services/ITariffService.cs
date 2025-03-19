using App.Application.DTO;

namespace App.Application.Interfaces.Services
{
    public interface ITariffService
    {
        Task AddTariffs(int routeId, List<TariffDTO> tariffs);
        Task<bool> RemoveTariffs(int routeId);
        Task<List<TariffDTO>> GetTariffs(int routeId);
        Task<TariffDTO> GetTariff(int tariffId);
        Task<bool> UpdateTariffs(int routeId, List<TariffDTO> tariffs);
    }
}
