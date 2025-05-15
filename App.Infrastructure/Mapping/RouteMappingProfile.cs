using App.Application.DTO;
using App.Core.Entities;
using AutoMapper;

namespace App.Infrastructure.Mapping
{
    public class RouteMappingProfile : Profile
    {
        public RouteMappingProfile()
        {
            CreateMap<Route, RouteSummaryDTO>();
            CreateMap<RouteSummaryDTO, Route>();
        }
    }
}
