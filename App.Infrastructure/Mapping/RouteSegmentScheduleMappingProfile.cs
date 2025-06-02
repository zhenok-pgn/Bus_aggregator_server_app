using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class RouteSegmentScheduleMappingProfile : Profile
    {
        public RouteSegmentScheduleMappingProfile()
        {
            CreateMap<RouteSegmentSchedule, RouteSegmentScheduleDTO>();
            CreateMap<RouteSegmentScheduleDTO, RouteSegmentSchedule>();
        }
    }
}
