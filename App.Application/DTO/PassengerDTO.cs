using App.Core.Enums;

namespace App.Application.DTO
{
    public class PassengerDTO
    {
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public required string Patronymic { get; set; }
        public DocumentType DocumentType { get; set; }
        public required string DocumentNumber { get; set; }
        public DateOnly DayOfBirth { get; set; }
        public Gender Gender { get; set; }
    }
}
