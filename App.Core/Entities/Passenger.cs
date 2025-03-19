namespace App.Core.Entities
{
    public class Passenger
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PassportNumber { get; set; }
        public required string Sex { get; set; }
    }
}
