using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class LocalityMappingProfile : Profile
    {
        public LocalityMappingProfile()
        {
            CreateMap<Locality, LocalityDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OsmId))
                .ForMember(dest => dest.Timezone, opt => opt.MapFrom(src => src.UtcTimezone.Name))
                .ForMember(dest => dest.OffsetMinutes, opt => opt.MapFrom(src => src.UtcTimezone.OffsetMinutes));
            CreateMap<LocalityDTO, Locality>();
        }
    }
}
