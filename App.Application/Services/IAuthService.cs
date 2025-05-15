using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.DTO.Responses;

namespace App.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest dto);
        Task<AuthResponse> RefreshAsync(string accessToken, string refreshToken);
        Task<UserDTO> GetMe();
        Task<AuthResponse> RegisterPassengerAsync(PassengerRegisterRequest dto);
        //Task<AuthResponse> RegisterCarrierAsync(CarrierRegisterRequest dto);
        //Task<AuthResponse> RegisterDriverAsync(DriverRegisterRequest dto);
    }
}
