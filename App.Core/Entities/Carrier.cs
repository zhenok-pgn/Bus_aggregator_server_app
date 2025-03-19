namespace App.Core.Entities
{
    public class Carrier
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Inn { get; set; }
        public required string Ogrn { get; set; }
        public required string Ogrnip { get; set; }
        public required string Address { get; set; }
        public required string OfficeHours { get; set; }
        public required string Phones { get; set; }
        public bool IsBan { get; set; }
        public required string HashedPassword { get; set; }
    }
}
