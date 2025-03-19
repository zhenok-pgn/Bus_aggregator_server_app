namespace App.Core.Entities
{
    public class Driver
    {
        public int Id { get; set; }
        public required string LicenseId { get; set; }
        public required string Name { get; set; }
        public bool IsBan { get; set; }
        public required string HashedPassword { get; set; }
    }
}
