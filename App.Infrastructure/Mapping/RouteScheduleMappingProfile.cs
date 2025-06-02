using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class RouteScheduleMappingProfile : Profile
    {
        public RouteScheduleMappingProfile()
        {
            CreateMap<RouteSchedule, RouteScheduleDTO>();
            CreateMap<RouteScheduleDTO, RouteSchedule>();

            CreateMap<RouteSchedule, RouteScheduleSummaryDTO>()
                .ForMember(dist=>dist.StartDate, opt=>opt.MapFrom(src=>src.SchedulePattern.StartDate))
                .ForMember(dist => dist.EndDate, opt => opt.MapFrom(src => src.SchedulePattern.EndDate))
                .ForMember(dist => dist.DaysOfWeek, opt => opt.MapFrom(src => src.SchedulePattern.DaysOfWeek));
        }
    }
}
