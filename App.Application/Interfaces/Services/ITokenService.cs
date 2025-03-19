using App.Application.DTO.Responses;
using System.Security.Claims;

namespace App.Application.Interfaces.Services
{
    public interface ITokenService
    {
        AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
        RefreshToken GenerateRefreshToken();
    }
}
