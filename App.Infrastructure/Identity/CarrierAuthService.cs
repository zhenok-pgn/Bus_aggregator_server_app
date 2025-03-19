using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.DTO.Responses;
using App.Application.Interfaces.Services;
using App.Core.Entities;
using App.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.Infrastructure.Identity
{
    public class CarrierAuthService : IAuthService<Carrier>
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        public CarrierAuthService(ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public string GetJwtSecurityToken(CarrierDTO user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Inn),
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

        public Task<AuthResponse> GetLoginResponse(AuthRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponse> GetRefreshResponse(RefreshTokenRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponse> GetSignupResponse(AuthRequest user)
        {
            throw new NotImplementedException();
        }

        /*public async Task<bool> Login(CarrierDTO user)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var userInDB = await db.Carriers.FirstOrDefaultAsync(p => p.Inn == user.Inn);
            if (userInDB is null) { return false; }

            return _passwordHasher.VerifyPassword(user.Password, userInDB.HashedPassword);
        }

        public async Task<bool> Signin(CarrierDTO user)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var userInDB = await db.Carriers.FirstOrDefaultAsync(p => p.Inn == user.Inn);
            if (userInDB is not null) { return false; }

            string hashedPassword = _passwordHasher.HashPassword(user.Password);
            await db.Carriers.AddAsync(new()
            {
                Name = user.Name,
                Inn = user.Inn,
                Ogrn = user.Ogrn,
                Ogrnip = user.Ogrnip,
                Address = user.Address,
                OfficeHours = user.OfficeHours,
                Phones = user.Phones,
                HashedPassword = hashedPassword
            });
            await db.SaveChangesAsync();

            return true;
        }*/
    }
}
