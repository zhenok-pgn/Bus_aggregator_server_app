using App.WEB.BLL.DTO.Responses;
using App.WEB.BLL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.WEB.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AccessToken GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenExpires = DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes"));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: tokenExpires,
                signingCredentials: creds
            );

            return new() { Token = new JwtSecurityTokenHandler().WriteToken(token), Expires = tokenExpires };
        }

        public RefreshToken GenerateRefreshToken()
        {
            return new() { Token = Guid.NewGuid().ToString(), Expires = DateTime.Now.AddDays(GetRefreshTokenExpirationDays()) };
        }

        private double GetRefreshTokenExpirationDays()
        {
            return _configuration.GetValue<double>("Jwt:RefreshTokenExpirationDays");
        }
    }
}
