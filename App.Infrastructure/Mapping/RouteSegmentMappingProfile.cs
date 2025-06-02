using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class RouteSegmentMappingProfile : Profile
    {
        public RouteSegmentMappingProfile()
        {
            CreateMap<RouteSegment, RouteSegmentDTO>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.FromStop))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.ToStop));
            CreateMap<RouteSegmentDTO, RouteSegment>()
                .ForMember(dest => dest.ToStop, opt => opt.MapFrom(src => src.To))
                .ForMember(dest => dest.FromStop, opt => opt.MapFrom(src => src.From));
        }
    }
}
