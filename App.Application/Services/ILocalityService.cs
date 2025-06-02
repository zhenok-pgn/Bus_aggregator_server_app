using App.Application.DTO;

namespace App.Application.Services
{
    public interface ILocalityService
    {
        Task<List<LocalityDTO>> GetLocalitiesAsync();
        Task<LocalityDTO> GetLocalityAsync(int localityId);
    }
}
