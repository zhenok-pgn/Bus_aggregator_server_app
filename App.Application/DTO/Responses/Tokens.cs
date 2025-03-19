namespace App.Application.DTO.Responses
{
    public record struct AccessToken(string Token, DateTimeOffset Expires);
    public record struct RefreshToken(string Token, DateTimeOffset Expires);
}
