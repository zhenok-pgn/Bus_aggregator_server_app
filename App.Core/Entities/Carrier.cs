namespace App.Core.Entities
{
    public class Carrier : User
    {
        public required string Name { get; set; }
        public required string Inn { get; set; }
        public required string Ogrn { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public string? Website { get; set; }
    }
}
