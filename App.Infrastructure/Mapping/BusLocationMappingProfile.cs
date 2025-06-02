using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class BusLocationMappingProfile : Profile
    {
        public BusLocationMappingProfile()
        {
            CreateMap<BusLocation, BusLocationDTO>();
            CreateMap<BusLocationDTO, BusLocation>();
        }
    }
}
