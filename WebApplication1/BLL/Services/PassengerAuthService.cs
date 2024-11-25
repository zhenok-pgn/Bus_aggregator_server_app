using App.BLL.DTO;
using App.BLL.Interfaces;
using App.DAL.EF;
using App.DAL.Entities;
using App.DAL.Infrastructure;
using App.WEB.BLL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace App.WEB.BLL.Services
{
    public class PassengerAuthService : IAuthService<PassengerDTO>
    {
        public async Task<PassengerDTO?> FindAndCheckUser(IFormCollection form)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var user = await db.Passengers.FirstOrDefaultAsync(p => p.Phone == form["phone"]);
            if (user is null) { return null; }

            if(RBKDF2PasswordHasher.VerifyPassword(form["password"], user.Password))
            {
                return user.MapToDto();
            }

            return null;
        }

        public async Task<bool> IsDataCorrect(IFormCollection form)
        {
            return form.ContainsKey("phone") || !form.ContainsKey("password");
        }

        public async Task<PassengerDTO?> GetUserIfExist(IFormCollection form)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var result = await db.Passengers.FirstOrDefaultAsync(p => p.Phone == form["phone"]);
            return result.MapToDto();
        }
    }
}
