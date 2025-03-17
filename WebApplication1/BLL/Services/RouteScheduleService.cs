using App.DAL.EF;
using App.DAL.Entities;
using App.WEB.BLL.DTO;
using App.WEB.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.WEB.BLL.Services
{
    public class RouteScheduleService : IRouteScheduleService
    {
        private readonly ITariffService _tariffService;

        public RouteScheduleService(ITariffService tariffService)
        {
            _tariffService = tariffService;
        }

        public async Task AddRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var schedules = routeSchedules.Select(rs => new RouteSchedule { RouteId = routeId, BaseSeatingPlan = rs.BaseSeatingPlan }).ToList();
            db.RouteSchedules.AddRange(schedules);
            await db.SaveChangesAsync();
        }

        public async Task<List<RouteScheduleDTO>> GetRouteSchedules(int routeId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var tariffs = await _tariffService.GetTariffs(routeId);
            return await db.RouteSchedules
                .Where(rs => rs.RouteId == routeId)
                .Select(rs => new RouteScheduleDTO { BaseSeatingPlan = rs.BaseSeatingPlan, Tariff = tariffs.FirstOrDefault(t => t.Id == rs.TariffId)}).ToListAsync();
        }

        public async Task<bool> RemoveRouteSchedules(int routeId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            db.RouteSchedules.RemoveRange(db.RouteSchedules);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRouteSchedules(int routeId, List<RouteScheduleDTO> routeSchedules)
        {
            await RemoveRouteSchedules(routeId);
            await AddRouteSchedules(routeId, routeSchedules);

            return true;
        }
    }
}
