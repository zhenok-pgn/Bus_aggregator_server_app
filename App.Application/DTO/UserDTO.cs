namespace App.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}
