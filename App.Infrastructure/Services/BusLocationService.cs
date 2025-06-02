using App.Application.DTO;
using App.Application.Services;
using App.Core.Entities;
using App.Core.Helpers;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace App.Infrastructure.Services
{
    public class BusLocationService : IBusLocationService
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly ITripNotifier _tripNotifier;
        public BusLocationService(ApplicationDBContext context, IMapper mapper, ITripNotifier tripNotifier)
        {
            _db = context;
            _mapper = mapper;
            _tripNotifier = tripNotifier;
        }

        public async Task<BusLocationDTO> GetLatestBusLocationAsync(int tripId)
        {
            var last = await _db.BusLocations
                .Where(x => x.TripId == tripId)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Местоположения автобуса по рейсу {tripId} не найдено");

            return _mapper.Map<BusLocationDTO>(last);
        }

        public async Task UpdateBusLocationAsync(int driverId, BusLocationDTO dto)
        {
            if (!GpsValidator.IsValidCoordinates(dto.Latitude, dto.Longitude))
                throw new ValidationException("Координаты вне допустимого диапазона");

            var location = _mapper.Map<BusLocation>(dto);
            var last = await _db.BusLocations
                .Where(x => x.TripId == location.TripId)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefaultAsync();

            if (last != null)
            {
                if (!GpsValidator.IsRealisticJump(last.Latitude, last.Longitude, dto.Latitude, dto.Longitude))
                    throw new ValidationException("Обнаружен нереалистичный скачок координат.");

                if (!GpsValidator.IsValidSpeed(last.Latitude, last.Longitude, last.Timestamp.ToUnixTimeMilliseconds(),
                                               dto.Latitude, dto.Longitude, dto.Timestamp.ToUnixTimeMilliseconds()))
                    throw new ValidationException("Скорость перемещения превышает допустимое значение.");
            }

            _db.BusLocations.Add(location);
            await _db.SaveChangesAsync();

            await _tripNotifier.SendLocationUpdateAsync(location.TripId, dto);
        }

        public async Task<double> GetBusAverageSpeedAsync(int tripId)
        {
            var lastLocations = await _db.BusLocations
                .Where(l => l.TripId == tripId)
                .OrderByDescending(l => l.Timestamp)
                .Take(10)
                .ToListAsync();

            var totalDistance = 0.0;
            for(var i = 1; i < lastLocations.Count; i++)
            {
                totalDistance += GpsValidator
                    .Haversine(lastLocations[i - 1].Latitude, lastLocations[i - 1].Longitude, lastLocations[i].Latitude, lastLocations[i].Longitude);
            }

            var totalTime = (lastLocations.Last().Timestamp - lastLocations.First().Timestamp).TotalSeconds;
            return totalDistance / totalTime;
        }
    }
}
