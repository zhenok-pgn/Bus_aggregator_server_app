namespace App.WEB.BLL.Infrastructure
{
    public class CookieSettings
    {
        public bool HttpOnly { get; set; }
        public bool Secure { get; set; }
        public required string SameSite { get; set; }
        public int ExpiresInDays { get; set; }
    }
}
