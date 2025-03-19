using App.Application.DTO.Requests;
using App.Application.DTO.Responses;

namespace App.Application.Interfaces.Services
{
    public interface IAuthService<T> where T : class
    {
        /// <summary>
        /// Проверяет, существует ли пользователь с логином
        /// </summary>
        /// <param name="formCollection"></pram>
        /// <returns></returns>
        //Task<T?> GetUserIfExist(IFormCollection form);
        Task<AuthResponse> GetLoginResponse(AuthRequest user);
        Task<AuthResponse> GetSignupResponse(AuthRequest user);
        Task<AuthResponse> GetRefreshResponse(RefreshTokenRequest user);
    }
}
