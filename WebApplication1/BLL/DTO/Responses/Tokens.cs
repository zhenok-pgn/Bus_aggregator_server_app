namespace App.WEB.BLL.DTO.Responses
{
    public record struct AccessToken(string Token, DateTimeOffset Expires);
    public record struct RefreshToken(string Token, DateTimeOffset Expires);
}
