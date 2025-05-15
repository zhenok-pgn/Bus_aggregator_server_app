using App.Application.Services;
using App.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SymmetricSecurityKey _key;
        private readonly string _securityAlgorithm;

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            _securityAlgorithm = SecurityAlgorithms.HmacSha256;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = GetClaims(user);

            var creds = new SigningCredentials(_key, _securityAlgorithm);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetUserIdFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(_securityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Convert.ToInt32(userId);
        }

        public RefreshToken GenerateRefreshToken(int userId)
        {
            return new() {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTimeOffset.UtcNow.AddDays(GetRefreshTokenExpirationDays()),
                UserId = userId
            };
        }

        private double GetRefreshTokenExpirationDays()
        {
            return _configuration.GetValue<double>("Jwt:RefreshTokenExpirationDays");
        }

        /*public string? GetUserIdFromContext()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }*/
        public int GetUserIdFromContext() 
        { 
            var nameIdentifier = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(nameIdentifier, out int userId))
            {
                return userId;
            }

            // Обработка случая, когда идентификатор не найден или не является числом
            throw new InvalidOperationException("User ID is not available or invalid.");
        }

    private static List<Claim> GetClaims(User user)
        {
            return new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }
    }
}
