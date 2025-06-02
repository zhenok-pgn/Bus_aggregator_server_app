using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class BusStopMappingProfile : Profile
    {
        public BusStopMappingProfile()
        {
            CreateMap<BusStop, BusStopDTO>();
            CreateMap<BusStopDTO, BusStop>();
        }
    }
}
