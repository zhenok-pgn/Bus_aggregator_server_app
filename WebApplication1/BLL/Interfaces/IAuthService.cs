using App.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Interfaces
{
    public interface IAuthService<T, H> where H : IPasswordHasher
    {
        /// <summary>
        /// Проверяет, существует ли пользователь с логином
        /// </summary>
        /// <param name="formCollection"></pram>
        /// <returns></returns>
        Task<T?> GetUserIfExist(IFormCollection form);
        Task<bool> Login(T user);
        Task<bool> Signin(T user);
        string GetJwtSecurityToken(T user);
    }
}
