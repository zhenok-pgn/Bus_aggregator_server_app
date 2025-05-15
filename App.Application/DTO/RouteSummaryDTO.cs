namespace App.Application.DTO
{
    public class RouteSummaryDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string RegistrationNumber { get; set; }
        public required string Number { get; set; }
        public int CarrierId { get; set; }
    }
}
