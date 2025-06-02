using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class SchedulePatternMappingProfile : Profile
    {
        public SchedulePatternMappingProfile()
        {
            CreateMap<SchedulePattern, SchedulePatternDTO>();
            CreateMap<SchedulePatternDTO, SchedulePattern>();
        }
    }
}
