using App.Application.DTO;
using App.Application.Services;
using App.Core.Entities;
using App.Core.Enums;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace App.Infrastructure.Services
{
    public class RouteService : IRouteService
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public RouteService(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task CreateAsync(RouteDTO dto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var route = new Route()
                {
                    Name = dto.Name,
                    RegistrationNumber = dto.RegistrationNumber,
                    Number = dto.Number,
                    CarrierId = dto.CarrierId,
                };
                _db.Routes.Add(route);
                await _db.SaveChangesAsync();

                foreach (var rs in dto.RouteSchedules)
                {
                    var schedulePattern = new SchedulePattern()
                    {
                        DaysOfWeek = (DaysOfWeekFlags)rs.SchedulePattern.DaysOfWeek,
                        StartDate = rs.SchedulePattern.StartDate,
                        EndDate = rs.SchedulePattern.EndDate
                    };
                    //попытка найти или сохранить schedulePattern

                    var existingSchedulePattern = await _db.SchedulePatterns
                        .Where(sp => sp.DaysOfWeek == schedulePattern.DaysOfWeek
                            && sp.StartDate == schedulePattern.StartDate
                            && sp.EndDate == schedulePattern.EndDate)
                        .FirstOrDefaultAsync();
                    if (existingSchedulePattern != null)
                    {
                        schedulePattern = existingSchedulePattern;
                    }
                    else
                    {
                        _db.SchedulePatterns.Add(schedulePattern);
                        await _db.SaveChangesAsync();
                    }

                    // сохранение расписания маршрута
                    var routeSchedule = new RouteSchedule()
                    {
                        DepartureTime = rs.DepartureTime,
                        SchedulePatternId = schedulePattern.Id,
                        RouteId = route.Id
                    };
                    _db.RouteSchedules.Add(routeSchedule);
                    await _db.SaveChangesAsync();

                    // сохранение расписания маршрута по сегментам
                    foreach (var rss in rs.RouteSegmentSchedules)
                    {

                        // проверка и сохранение остановок
                        var busStopFrom = await _db.BusStops
                            .FirstOrDefaultAsync(bs => bs.Latitude == rss.RouteSegment.From.Latitude && bs.Longitude == rss.RouteSegment.From.Longitude);
                        if (busStopFrom == null)
                        {
                            var dtoLocalityFrom = rss.RouteSegment.From.Locality;
                            busStopFrom = new BusStop()
                            {
                                Name = rss.RouteSegment.From.Name,
                                Locality = await GetOrCreateLocality(dtoLocalityFrom),
                                Address = rss.RouteSegment.From.Address,
                                Latitude = rss.RouteSegment.From.Latitude,
                                Longitude = rss.RouteSegment.From.Longitude,
                            };
                            _db.BusStops.Add(busStopFrom);
                            await _db.SaveChangesAsync();
                        }

                        var busStopTo = await _db.BusStops
                            .FirstOrDefaultAsync(bs => bs.Latitude == rss.RouteSegment.To.Latitude && bs.Longitude == rss.RouteSegment.To.Longitude);
                        if (busStopTo == null)
                        {
                            var dtoLocalityTo = rss.RouteSegment.To.Locality;
                            busStopTo = new BusStop()
                            {
                                Name = rss.RouteSegment.To.Name,
                                Locality = await GetOrCreateLocality(dtoLocalityTo),
                                Address = rss.RouteSegment.To.Address,
                                Latitude = rss.RouteSegment.To.Latitude,
                                Longitude = rss.RouteSegment.To.Longitude,
                            };
                            _db.BusStops.Add(busStopTo);
                            await _db.SaveChangesAsync();
                        }

                        // проверка и сохранение сегмента
                        var routeSegment = await _db.RouteSegments
                            .FirstOrDefaultAsync(rs => rs.FromStopId == busStopFrom!.Id && rs.ToStopId == busStopTo!.Id);
                        if (routeSegment == null)
                        {
                            routeSegment = new RouteSegment()
                            {
                                FromStopId = busStopFrom!.Id,
                                ToStopId = busStopTo!.Id,
                            };
                            _db.RouteSegments.Add(routeSegment);
                            await _db.SaveChangesAsync();
                        }

                        // сохранение расписания сегмента
                        // найти последнюю версию расписания (если есть)
                        /*var lastVersion = await _db.RouteSegmentSchedules
                            .Where(rss => rss.RouteSegmentId == routeSegment.Id)
                            .OrderByDescending(rss => rss.Version)
                            .Select(rss => rss.Version)
                            .FirstOrDefaultAsync();*/

                        var routeSegmentSchedule = new RouteSegmentSchedule()
                        {
                            RouteSegmentId = routeSegment.Id,
                            RouteScheduleId = routeSchedule.Id,
                            DepartureTime = rss.DepartureTime,
                            ArrivalTime = rss.ArrivalTime,
                            SegmentNumber = rss.SegmentNumber,
                            ArrivalDayNumber = rss.ArrivalDayNumber,
                            Price = rss.Price,
                            Version = 1
                        };
                        _db.RouteSegmentSchedules.Add(routeSegmentSchedule);
                        await _db.SaveChangesAsync();
                    }
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task DeleteAsync(int carrierId, int routeId)
        {
            throw new NotImplementedException();
        }

        public Task<RouteDTO> GetRouteAsync(int carrierId, int routeId)
        {
            throw new NotImplementedException();
            /*var route = await _db.Routes
    .Include(r => r.Carrier)
    .Include(r => r.RouteSchedules)
        .ThenInclude(rs => rs.SchedulePattern)
    .Include(r => r.RouteSchedules)
        .ThenInclude(rs => rs.RouteSegmentSchedules)
            .ThenInclude(rss => rss.RouteSegment)
                .ThenInclude(rs => rs.FromStop)
                    .ThenInclude(bs => bs.Locality)
                        .ThenInclude(l => l.UtcTimezone)
    .Include(r => r.RouteSchedules)
        .ThenInclude(rs => rs.RouteSegmentSchedules)
            .ThenInclude(rss => rss.RouteSegment)
                .ThenInclude(rs => rs.ToStop)
                    .ThenInclude(bs => bs.Locality)
                        .ThenInclude(l => l.UtcTimezone)
    .AsNoTracking() // Добавляем для read-only операций
    .FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
                return null;

            return new RouteDTO
            {
                Id = route.Id.ToString(),
                Name = route.Name,
                RegistrationNumber = route.RegistrationNumber,
                Number = route.Number,
                CarrierId = route.CarrierId,
                RouteSchedules = route.RouteSchedules.Select(rs => new RouteScheduleDTO
                {
                    Id = rs.Id.ToString(),
                    DepartureTime = rs.DepartureTime,
                    SchedulePattern = new SchedulePatternDTO
                    {
                        Id = rs.SchedulePattern.Id.ToString(),
                        DaysOfWeek = rs.SchedulePattern.DaysOfWeek,
                        StartDate = rs.SchedulePattern.StartDate,
                        EndDate = rs.SchedulePattern.EndDate
                    },
                    RouteSegmentSchedules = rs.RouteSegmentSchedules.Select(rss => new RouteSegmentScheduleDTO
                    {
                        Id = rss.Id.ToString(),
                        DepartureTime = rss.DepartureTime,
                        ArrivalTime = rss.ArrivalTime,
                        SegmentNumber = rss.SegmentNumber,
                        ArrivalDayNumber = rss.ArrivalDayNumber,
                        Price = rss.Price,
                        RouteSegment = new RouteSegmentDTO
                        {
                            Id = rss.RouteSegment.Id.ToString(),
                            From = new BusStopDTO
                            {
                                Id = rss.RouteSegment.FromStop.Id.ToString(),
                                Name = rss.RouteSegment.FromStop.Name,
                                Address = rss.RouteSegment.FromStop.Address,
                                Latitude = rss.RouteSegment.FromStop.Latitude,
                                Longitude = rss.RouteSegment.FromStop.Longitude,
                                Locality = new LocalityDTO
                                {
                                    OsmId = rss.RouteSegment.FromStop.Locality.OsmId,
                                    Name = rss.RouteSegment.FromStop.Locality.Name,
                                    Region = rss.RouteSegment.FromStop.Locality.Region,
                                    Country = rss.RouteSegment.FromStop.Locality.Country,
                                    District = rss.RouteSegment.FromStop.Locality.District,
                                    Timezone = rss.RouteSegment.FromStop.Locality.UtcTimezoneName,
                                    OffsetMinutes = rss.RouteSegment.FromStop.Locality.UtcTimezone?.OffsetMinutes ?? 0
                                }
                            },
                            To = new BusStopDTO
                            {
                                Id = rss.RouteSegment.ToStop.Id.ToString(),
                                Name = rss.RouteSegment.ToStop.Name,
                                Address = rss.RouteSegment.ToStop.Address,
                                Latitude = rss.RouteSegment.ToStop.Latitude,
                                Longitude = rss.RouteSegment.ToStop.Longitude,
                                Locality = new LocalityDTO
                                {
                                    OsmId = rss.RouteSegment.ToStop.Locality.OsmId,
                                    Name = rss.RouteSegment.ToStop.Locality.Name,
                                    Region = rss.RouteSegment.ToStop.Locality.Region,
                                    Country = rss.RouteSegment.ToStop.Locality.Country,
                                    District = rss.RouteSegment.ToStop.Locality.District,
                                    Timezone = rss.RouteSegment.ToStop.Locality.UtcTimezoneName,
                                    OffsetMinutes = rss.RouteSegment.ToStop.Locality.UtcTimezone?.OffsetMinutes ?? 0
                                }
                            }
                        }
                    }).ToList(),
                    Trips = rs.Trips.Select(t => new TripDTO
                    {
                        Id = t.Id.ToString(),
                        DepartureDate = t.DepartureDate,
                        TripStatus = t.TripStatus,
                        BusId = t.BusId,
                        DriverId = t.DriverId
                    }).ToList()
                }).ToList()
            };*/
        }

        public async Task<List<RouteSummaryDTO>> GetRoutesAsync(int carrierId)
        {
            var routes = await _db.Routes
            .Where(r => r.CarrierId == carrierId)
            .ToListAsync();

            return _mapper.Map<List<RouteSummaryDTO>>(routes);
        }

        public async Task<List<RouteScheduleSummaryDTO>> GetRouteSchedulesAsync(int carrierId)
        {
            var utcToday = DateOnly.FromDateTime(DateTime.UtcNow);

            var routeSchedules = await _db.RouteSchedules
                .Include(rs=>rs.Route)
                .Include(rs=>rs.SchedulePattern)
                .Where(rs=> 
                    rs.Route != null && 
                    rs.Route.CarrierId == carrierId &&
                    rs.SchedulePattern != null &&
                    rs.SchedulePattern.EndDate >= utcToday.AddDays(-1)) // Запас в 1 день
                .ToListAsync();

            return _mapper.Map<List<RouteScheduleSummaryDTO>>(routeSchedules);
        }

        public Task UpdateAsync(RouteDTO dto)
        {
            throw new NotImplementedException();
        }

        private async Task<Locality> GetOrCreateLocality(LocalityDTO dto)
        {
            var locality = await _db.Localities
                .FirstOrDefaultAsync(l => l.OsmId == dto.Id);

            if (locality == null)
            {
                locality = new Locality()
                {
                    OsmId = dto.Id,
                    Name = dto.Name,
                    Region = dto.Region,
                    Country = dto.Country,
                    District = dto.District,
                    UtcTimezoneName = dto.Timezone,
                    UtcTimezone = await GetOrCreateTimezone(dto.Timezone, dto.OffsetMinutes)
                };
                _db.Localities.Add(locality);
            }

            return locality;
        }

        private async Task<UtcTimezone> GetOrCreateTimezone(string name, int offset)
        {
            var zone = await _db.UtcTimezones
                .FirstOrDefaultAsync(l => l.Name == name);

            if (zone == null)
            {
                zone = new UtcTimezone()
                {
                    Name = name,
                    OffsetMinutes = offset
                };
                _db.UtcTimezones.Add(zone);
            }

            return zone;
        }
    }
}
