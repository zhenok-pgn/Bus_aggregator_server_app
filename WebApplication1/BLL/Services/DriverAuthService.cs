using App.BLL.Interfaces;
using App.DAL.EF;
using App.DAL.Infrastructure;
using App.BLL.DTO;
using App.WEB.BLL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.WEB.BLL.Services
{
    public class DriverAuthService : IAuthService<DriverDTO, RBKDF2PasswordHasher>
    {
        public async Task<bool> Login(DriverDTO user)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var userInDB = await db.Drivers.FirstOrDefaultAsync(p => p.LicenseId == user.LicenseId);
            if (userInDB is null) { return false; }

            return RBKDF2PasswordHasher.VerifyPassword(user.Password, userInDB.HashedPassword);
        }

        public async Task<bool> Signin(DriverDTO user)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var userInDB = await db.Drivers.FirstOrDefaultAsync(p => p.LicenseId == user.LicenseId);
            if (userInDB is not null) { return false; }

            string hashedPassword = RBKDF2PasswordHasher.HashPassword(user.Password);
            await db.Drivers.AddAsync(new()
            {
                Name = user.Name,
                LicenseId = user.LicenseId,
                HashedPassword = hashedPassword
            });
            await db.SaveChangesAsync();

            return true;
        }

        public Task<DriverDTO?> GetUserIfExist(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public string GetJwtSecurityToken(DriverDTO user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.LicenseId),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
