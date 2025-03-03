using App.DAL.Interfaces;
using App.WEB.BLL.DTO.Requests;
using App.WEB.BLL.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Interfaces
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
