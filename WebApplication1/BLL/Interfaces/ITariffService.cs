using App.BLL.DTO;
using App.WEB.BLL.DTO;

namespace App.WEB.BLL.Interfaces
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
