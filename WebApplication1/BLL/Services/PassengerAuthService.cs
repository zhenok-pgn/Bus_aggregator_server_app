using App.BLL.DTO;
using App.BLL.Interfaces;
using App.DAL.EF;
using App.DAL.Entities;
using App.DAL.Infrastructure;
using App.DAL.Interfaces;
using App.WEB.BLL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.WEB.BLL.Services
{
    public class PassengerAuthService : IAuthService<PassengerDTO, RBKDF2PasswordHasher>
    {
        public async Task<bool> Login(PassengerDTO user)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var userInDB = await db.Passengers.FirstOrDefaultAsync(p => p.Phone == user.Phone);
            if (userInDB is null) { return false; }

            return RBKDF2PasswordHasher.VerifyPassword(user.Password, userInDB.HashedPassword);
        }

        public async Task<bool> Signin(PassengerDTO user)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var userInDB = await db.Passengers.FirstOrDefaultAsync(p => p.Phone == user.Phone);
            if (userInDB is not null) { return false; }

            string hashedPassword = RBKDF2PasswordHasher.HashPassword(user.Password);
            await db.Passengers.AddAsync(new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                HashedPassword = hashedPassword
            });
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<PassengerDTO?> GetUserIfExist(IFormCollection form)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var result = await db.Passengers.FirstOrDefaultAsync(p => p.Phone == form["phone"]);
            return result.MapToDto();
        }

        public string GetJwtSecurityToken(PassengerDTO user)
        {
            var claims = new List<Claim> { 
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Phone),
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
