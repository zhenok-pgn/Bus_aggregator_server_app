using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.DTO.Responses;
using App.Application.Services;
using App.Core.Entities;
using App.Core.Enums;
using App.Core.Interfaces;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;

namespace App.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDBContext _db;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _hasher;
        private readonly IMapper _mapper;

        public AuthService(ApplicationDBContext db, ITokenService tokenGen, IPasswordHasher hasher, IMapper mapper)
        {
            _db = db;
            _tokenService = tokenGen;
            _hasher = hasher;
            _mapper = mapper;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == dto.Username);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var passwordValid = _hasher.Verify(dto.Password, user.HashedPassword);
            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid password");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            _db.RefreshTokens.RemoveRange(_db.RefreshTokens.Where(x => x.UserId == user.Id));
            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();

            return new() { 
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token, 
                RefreshTokenExpiresAt = refreshToken.Expires
            };
        }

        public async Task<AuthResponse> RefreshAsync(string accessToken, string refreshToken)
        {
            var userId = _tokenService.GetUserIdFromExpiredToken(accessToken);
            var storedRefreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (storedRefreshToken == null || storedRefreshToken.UserId != userId || storedRefreshToken.Expires <= DateTime.Now)
                throw new SecurityTokenException("Invalid refresh token");

            var user = await _db.Users.FindAsync(userId);
            if(user == null)
                throw new UnauthorizedAccessException("User not found");

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id);

            storedRefreshToken.Token = newRefreshToken.Token;
            storedRefreshToken.Expires = newRefreshToken.Expires;
            await _db.SaveChangesAsync();

            return new AuthResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresAt = newRefreshToken.Expires,
            };
        }

        public async Task<UserDTO> GetMe()
        {
            var userId = _tokenService.GetUserIdFromContext();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            return new UserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Role = user.Role.ToString(),
            };
        }

        public async Task<bool> RegisterCarrierAsync(CarrierRegisterRequest dto)
        {
            var userInDb = await _db.Users.FirstOrDefaultAsync(p => p.UserName == dto.Username);
            if (userInDb != null)
                throw new UnauthorizedAccessException("User already exists");
            
            var carrier = new Carrier
            {
                Name = dto.Name,
                Inn = dto.Inn,
                Ogrn = dto.Ogrn,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                UserName = dto.Username,
                HashedPassword = _hasher.Hash(dto.Password),
                Role = UserRole.Carrier,

            };

            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                _db.Carriers.Add(carrier);

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return true;
        }

        public async Task<bool> RegisterDriverAsync(DriverRegisterRequest dto)
        {
            var userInDb = await _db.Users.FirstOrDefaultAsync(p => p.UserName == dto.Username);
            if (userInDb != null)
                throw new UnauthorizedAccessException("User already exists");

            /*var driver = new Driver
            {
                UserName = dto.Username,
                HashedPassword = _hasher.Hash(dto.Password),
                Role = UserRole.Driver,
                Surname = dto.Surname,
                Name = dto.Name,
                Patronymic = dto.Patronymic,
                LicenseNumber = dto.LicenseNumber,
                EmployeeNumber = dto.EmployeeNumber,
                DayOfBirth = dto.DayOfBirth,
                CarrierId = dto.CarrierId,
            };*/
            
            var driver = _mapper.Map<Driver>(dto);
            driver.HashedPassword = _hasher.Hash(dto.Password);

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                _db.Drivers.Add(driver);

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return true;
        }

        public async Task<AuthResponse> RegisterPassengerAsync(PassengerRegisterRequest dto)
        {
            var userInDb = await _db.Users.FirstOrDefaultAsync(p => p.UserName == dto.Username);
            if (userInDb != null)
                throw new UnauthorizedAccessException("User already exists");

            /*var user = new User
            {
                UserName = dto.Username,
                HashedPassword = _hasher.Hash(dto.Password),
                Role = UserRole.Passenger
            };*/

            var buyer = new Buyer
            {
                Surname = dto.Surname,
                Name = dto.Name,
                Patronymic = dto.Patronymic,
                Phone = dto.Phone,
                UserName = dto.Username,
                HashedPassword = _hasher.Hash(dto.Password),
                Role = UserRole.Passenger,
            };

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                /*_db.Users.Add(user);
                await _db.SaveChangesAsync();*/

                _db.Buyers.Add(buyer);

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            var accessToken = _tokenService.GenerateAccessToken(buyer);
            var refreshToken = _tokenService.GenerateRefreshToken(buyer.Id);

            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();

            return new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.Expires
            };
        }
    }
}
