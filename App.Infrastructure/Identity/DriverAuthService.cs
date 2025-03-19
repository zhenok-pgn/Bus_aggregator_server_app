using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Application.DTO.Requests;
using App.Application.DTO.Responses;
using App.Application.DTO;
using App.Core.Entities;
using App.Application.Interfaces.Services;
using App.Core.Interfaces;

namespace App.Infrastructure.Identity
{
    public class DriverAuthService : IAuthService<Driver>
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        public DriverAuthService(ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        /*public async Task<bool> Login(DriverDTO user)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var userInDB = await db.Drivers.FirstOrDefaultAsync(p => p.LicenseId == user.LicenseId);
            if (userInDB is null) { return false; }

            return _passwordHasher.VerifyPassword(user.Password, userInDB.HashedPassword);
        }

        public async Task<bool> Signin(DriverDTO user)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var userInDB = await db.Drivers.FirstOrDefaultAsync(p => p.LicenseId == user.LicenseId);
            if (userInDB is not null) { return false; }

            string hashedPassword = _passwordHasher.HashPassword(user.Password);
            await db.Drivers.AddAsync(new()
            {
                Name = user.Name,
                LicenseId = user.LicenseId,
                HashedPassword = hashedPassword
            });
            await db.SaveChangesAsync();

            return true;
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
        }*/

        public Task<AuthResponse> GetLoginResponse(AuthRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponse> GetSignupResponse(AuthRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponse> GetRefreshResponse(RefreshTokenRequest user)
        {
            throw new NotImplementedException();
        }
    }
}
