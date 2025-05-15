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
        }
    }
}
