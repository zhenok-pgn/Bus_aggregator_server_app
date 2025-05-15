using App.Core.Enums;

namespace App.Core.Entities
{
    // определяет конкретный выезд по маршруту
    public class Trip
    {
        public int Id { get; set; }
        public int RouteScheduleId { get; set; }
        public RouteSchedule? RouteSchedule { get; set; }
        public int BusId { get; set; }
        public Bus? Bus { get; set; }
        public int DriverId { get; set; }
        public Driver? Driver { get; set; }
        public DateOnly DepartureDate { get; set; }
        public TripStatus TripStatus { get; set; }
    }
}
