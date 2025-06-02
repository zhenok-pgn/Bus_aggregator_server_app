using App.Application.DTO;
using App.Core.Entities;

namespace App.Application.Services
{
    public interface IRouteSegmentScheduleService
    {
        Task<List<RouteSegmentScheduleDTO>> GetSequentialSegments(int tripId);
        Task<List<int>> GetRelatedSegments(RouteSegmentSchedule targetSegment);
        Task<int> GetDepartureDayNumber(RouteSegmentSchedule targetSegment);
        Task<List<int>> GetSegmentsFromFirstStop(int tripId);
    }
}
