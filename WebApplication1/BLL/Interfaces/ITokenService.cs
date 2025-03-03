using App.WEB.BLL.DTO.Responses;
using System.Security.Claims;

namespace App.WEB.BLL.Interfaces
{
    public interface ITokenService
    {
        AccessToken GenerateAccessToken(IEnumerable<Claim> claims);
        RefreshToken GenerateRefreshToken();
    }
}
