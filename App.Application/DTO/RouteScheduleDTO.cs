using App.Core.Entities;

namespace App.Application.DTO
{
    public class RouteScheduleDTO
    {
        public required string Id { get; set; }
        public required SchedulePatternDTO SchedulePattern { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public required List<RouteSegmentScheduleDTO> RouteSegmentSchedules { get; set; }
    }
}
