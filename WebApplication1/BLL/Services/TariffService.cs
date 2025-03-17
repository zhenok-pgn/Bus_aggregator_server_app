using App.DAL.EF;
using App.DAL.Entities;
using App.WEB.BLL.DTO;
using App.WEB.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.WEB.BLL.Services
{
    public class TariffService : ITariffService
    {
        private readonly IRouteStopService _routeStopService;

        public TariffService(IRouteStopService routeStopService)
        {
            _routeStopService = routeStopService;
        }

        public async Task AddTariffs(int routeId, List<TariffDTO> tariffs)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            var tariffEntities = tariffs.Select(t => new Tariff { RouteId = routeId, Name = t.Name, Currency = t.Currency }).ToList();
            db.Tariffs.AddRange(tariffEntities);
            await db.SaveChangesAsync();
        }

        public async Task<TariffDTO> GetTariff(int tariffId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            // Загружаем Tariff и связанные RouteSegmentPrice
            var tariff = await db.Tariffs
                .Include(t => t.Prices) // Включаем связанные данные RouteSegmentPrice
                .FirstOrDefaultAsync(t => t.Id == tariffId);

            if (tariff == null)
            {
                throw new Exception($"Tariff with id {tariffId} not found.");
            }

            // Преобразуем Tariff в TariffDTO
            var tariffDto = new TariffDTO
            {
                Id = tariff.Id,
                Name = tariff.Name,
                Currency = tariff.Currency,
                Prices = new List<RouteSegmentPriceDTO>()
            };

            // Загружаем данные RouteStopDTO через RouteStopService
            foreach (var price in tariff.Prices)
            {
                var routeStopFromDto = await _routeStopService.GetRouteStop(price.RouteStopFromId);
                var routeStopToDto = await _routeStopService.GetRouteStop(price.RouteStopToId);

                tariffDto.Prices.Add(new RouteSegmentPriceDTO
                {
                    Id = price.Id,
                    RouteStopFrom = routeStopFromDto,
                    RouteStopTo = routeStopToDto,
                    Price = price.Price
                });
            }

            return tariffDto;
        }

        public async Task<List<TariffDTO>> GetTariffs(int routeId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            return await db.Tariffs
                .Where(ts => ts.RouteId == routeId)
                .Select(t => new TariffDTO { , Name = t.Name, Currency = t.Currency }).ToListAsync();
        }

        public async Task<bool> RemoveTariffs(int routeId)
        {
            using ApplicationDBContext db = new ApplicationDBContext();
            db.Tariffs.RemoveRange(db.Tariffs.Where(ts => ts.RouteId == routeId));
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTariffs(int routeId, List<TariffDTO> tariffs)
        {
            await RemoveTariffs(routeId);
            await AddTariffs(routeId, tariffs);

            return true;
        }
    }
}
