using App.Application.DTO;
using App.Application.Interfaces;
using App.Application.Interfaces.Services;
using App.Core.Entities;

namespace App.Application.Services
{
    public class TariffService : ITariffService
    {
        private readonly IRouteStopService _routeStopService;
        private readonly IUnitOfWork _unitOfWork;

        public TariffService(IRouteStopService routeStopService, IUnitOfWork unitOfWork)
        {
            _routeStopService = routeStopService;
            _unitOfWork = unitOfWork;
        }

        public async Task AddTariffs(int routeId, List<TariffDTO> tariffs)
        {
            var tariffEntities = tariffs.Select(t => new Tariff { RouteId = routeId, Name = t.Name, Currency = t.Currency }).ToList();
            await _unitOfWork.Tariffs.AddRangeAsync(tariffEntities);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TariffDTO> GetTariff(int tariffId)
        {
            // Загружаем Tariff и связанные RouteSegmentPrice
            var tariff = await _unitOfWork.Tariffs
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
            var tariffs = await _unitOfWork.Tariffs
                .FindAsync(ts => ts.RouteId == routeId);
            return tariffs
                .Select(t => new TariffDTO {Prices = new List<RouteSegmentPriceDTO>(), Name = t.Name, Currency = t.Currency }).ToList();
        }

        public async Task<bool> RemoveTariffs(int routeId)
        {
            await _unitOfWork.Tariffs.RemoveRange(await _unitOfWork.Tariffs.FindAsync(ts => ts.RouteId == routeId));
            await _unitOfWork.SaveChangesAsync();

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
