using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Проверка на правильность отправленных данных при авторизации
        /// </summary>
        /// <returns>True, if correct</returns>
        bool IsDataCorrect(IFormCollection );

        /// <summary>
        /// Находит пользователя 
        /// </summary>
        /// <returns></returns>
        object FindUser();
    }
}
