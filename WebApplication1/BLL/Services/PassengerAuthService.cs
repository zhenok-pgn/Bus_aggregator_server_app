using App.BLL.DTO;
using App.BLL.Interfaces;
using App.DAL.EF;
using App.DAL.Entities;
using App.DAL.Infrastructure;
using App.DAL.Interfaces;
using App.WEB.BLL.DTO.Requests;
using App.WEB.BLL.DTO.Responses;
using App.WEB.BLL.Infrastructure;
using App.WEB.BLL.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.WEB.BLL.Services
{
    public class PassengerAuthService : IAuthService<Passenger>
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        public PassengerAuthService(ITokenService tokenService, IPasswordHasher passwordHasher) 
        { 
            _tokenService = tokenService; 
            _passwordHasher = passwordHasher;
        }

        /*public async Task<bool> Login(DTO.Requests.LoginRequest request)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var userInDB = await db.Users.FirstOrDefaultAsync(p => p.UserName == request.Username);
            if (userInDB is null) { return false; }

            return RBKDF2PasswordHasher.VerifyPassword(request.Password, userInDB.HashedPassword);
        }*/

        public async Task<AuthResponse> GetLoginResponse(AuthRequest request)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var userInDB = db.Users.FirstOrDefault(p => p.UserName == request.Username);

            // валидация
            // TODO: catch FormatException
            if(userInDB is null || !_passwordHasher.VerifyPassword(request.Password, userInDB.HashedPassword))
                return null;

            // создание accessToken и refreshToken
            var claims = GetClaims(userInDB);
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // добавление refreshToken в бд
            // TODO: тоже эксептион ловить надо
            await db.RefreshTokens.AddAsync(new()
            {
                Token = refreshToken.Token,
                User = userInDB,
                Expires = refreshToken.Expires
            });
            await db.SaveChangesAsync();

            return new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> GetSignupResponse(AuthRequest request)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var userInDB = db.Users.FirstOrDefault(p => p.UserName == request.Username);

            if (userInDB is not null) { return null; }

            var user = new User()
            {
                UserName = request.Username,
                HashedPassword = _passwordHasher.HashPassword(request.Password),
                Role = "passenger"
            };
            // TODO: тоже эксептион ловить надо
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            // создание accessToken и refreshToken
            var claims = GetClaims(user);
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // добавление refreshToken в бд
            // TODO: тоже эксептион ловить надо
            await db.RefreshTokens.AddAsync(new()
            {
                Token = refreshToken.Token,
                User = user,
                Expires = refreshToken.Expires
            });
            await db.SaveChangesAsync();

            return new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> GetRefreshResponse(RefreshTokenRequest request)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var token = db.RefreshTokens.Include(r => r.User).FirstOrDefault(p => p.Token == request.RefreshToken);

            // валидация
            if (token is null || token.Expires < DateTime.Now)
                return null;

            // создание accessToken и refreshToken
            var claims = GetClaims(token.User);
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // изменение refreshToken
            token.Token = refreshToken.Token;
            token.Expires = refreshToken.Expires;
            await db.SaveChangesAsync();

            return new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        /*public async Task<bool> Signin(AuthRequest user)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var userInDB = await db.Passengers.FirstOrDefaultAsync(p => p.Phone == user.Phone);
            if (userInDB is not null) { return false; }

            string hashedPassword = _passwordHasher.HashPassword(user.Password);
            await db.Passengers.AddAsync(new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                HashedPassword = hashedPassword
            });
            await db.SaveChangesAsync();

            return true;
        }*/

        /*public async Task<PassengerDTO?> GetUserIfExist(IFormCollection form)
        {
            using ApplicationMysqlContext db = new ApplicationMysqlContext();
            var result = await db.Passengers.FirstOrDefaultAsync(p => p.Phone == form["phone"]);
            return result.MapToDto<PassengerDTO>();
        }*/

        private static List<Claim> GetClaims(User user)
        {
            return new List<Claim> { 
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };
        }
    }
}
