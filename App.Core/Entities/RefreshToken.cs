namespace App.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
