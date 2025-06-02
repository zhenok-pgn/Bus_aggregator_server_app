namespace App.Application.DTO
{
    public class TripExecutionDTO
    {
        public required TripDTO Trip { get; set; }
        public required List<TripExecutionSegmentDTO> SegmentExecutions { get; set; }
    }

    public class TripExecutionSegmentDTO
    {
        public required RouteSegmentScheduleDTO Segment { get; set; }
        public DateTimeOffset? Departure { get; set; }
        public DateTimeOffset? Arrival { get; set; }
        public string Status { get; set; } = SegmentExecutionStatus.Planned.ToString();
    }

    public enum SegmentExecutionStatus
    {
        Planned,
        InProgress,
        Completed,
    }
}