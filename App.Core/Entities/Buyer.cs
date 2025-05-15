namespace App.Core.Entities
{
    public class Buyer : User
    {
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
    }
}
