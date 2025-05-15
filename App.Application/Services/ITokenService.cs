using App.Core.Entities;
namespace App.Application.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(int userId);
        int GetUserIdFromExpiredToken(string accessToken);
        int GetUserIdFromContext();
    }
}
