namespace App.WEB.BLL.DTO.Responses
{
    public class AuthResponse
    {
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public DateTimeOffset AccessTokenExpires { get; set; }
    }
}
