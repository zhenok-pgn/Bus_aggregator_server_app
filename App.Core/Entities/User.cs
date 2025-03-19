namespace App.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string HashedPassword { get; set; }
        public required string Role { get; set; }
        public bool IsBan { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
