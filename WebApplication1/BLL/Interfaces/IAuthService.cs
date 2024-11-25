using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Interfaces
{
    public interface IAuthService<T>
    {
        /// <summary>
        /// Проверка на правильность отправленных данных при авторизации
        /// </summary>
        /// <returns>True, if correct</returns>
        Task<bool> IsDataCorrect(IFormCollection form);

        /// <summary>
        /// Находит пользователя и проверяет корректность логина и пароля
        /// </summary>
        /// <returns></returns>
        Task<T?> FindAndCheckUser(IFormCollection form);

        /// <summary>
        /// Проверяет, существует ли пользователь с логином
        /// </summary>
        /// <param name="formCollection"></pram>
        /// <returns></returns>
        Task<T?> GetUserIfExist(IFormCollection form);
    }
}
