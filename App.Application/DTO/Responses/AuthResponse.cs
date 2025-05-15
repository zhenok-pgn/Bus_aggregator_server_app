namespace App.Application.DTO.Responses
{
    public class AuthResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpiresAt { get; set; }
    }
}
