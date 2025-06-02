using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.Services;
using App.Core.Entities;
using App.Core.Enums;
using App.Core.Helpers;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace App.Infrastructure.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly IRouteSegmentScheduleService _routeSegmentScheduleService;
        private readonly ITicketService _ticketService;

        public TripService(ApplicationDBContext db, IMapper mapper, IRouteSegmentScheduleService routeSegmentScheduleService, ITicketService ticketService)
        {
            _db = db;
            _mapper = mapper;
            _routeSegmentScheduleService = routeSegmentScheduleService;
            _ticketService = ticketService;
        }
        public async Task<List<TripDTO>> GetTripsAsync(int? carrierId, int? localityFromId, int? localityToId, DateOnly? departureDateFrom, DateOnly? departureDateTo, List<int> routeIds)
        {
            if (localityFromId.HasValue && !localityToId.HasValue || !localityFromId.HasValue && localityToId.HasValue)
                throw new ArgumentException("Both localityFromId and localityToId must be provided or neither.");

            List<int>? segmentIds = null;
            if (localityFromId.HasValue)
            {
                segmentIds = _db.RouteSegments
                    .Where(rs => rs.FromStop.LocalityOsmId == localityFromId &&
                                    rs.ToStop.LocalityOsmId == localityToId)
                    .Select(rs => rs.Id)
                    .ToList();
            }

            var query = _db.Trips
                .Include(t => t.RouteSchedule)
                    .ThenInclude(rs => rs.Route)
                .Include(t => t.Bus)
                    .ThenInclude(b=>b.Seats)
                .Include(t => t.Driver)
                .AsQueryable();

            // Фильтрация по перевозчику
            if (carrierId.HasValue)
            {
                query = query.Where(t => t.RouteSchedule.Route.CarrierId == carrierId.Value);
            }

            // Фильтрация по дате отправления
            if (departureDateFrom.HasValue)
            {
                query = query.Where(t => t.DepartureDate >= departureDateFrom.Value);
            }

            if (departureDateTo.HasValue)
            {
                query = query.Where(t => t.DepartureDate <= departureDateTo.Value);
            }

            // Фильтрация по списку маршрутов
            if (routeIds != null && routeIds.Any())
            {
                query = query.Where(t => routeIds.Contains(t.RouteSchedule.RouteId));
            }

            var trips = await query
                .OrderBy(t => t.DepartureDate)
                .ThenBy(t => t.RouteSchedule.Route.Number)
                .ToListAsync();

            var tripsDT0 = new List<TripDTO>();
            foreach (var trip in trips)
            {
                RouteSegmentSchedule? schedule = null;

                // Фильтрация по сегменту маршрута
                if (segmentIds != null)
                {
                    schedule = await _db.RouteSegmentSchedules
                        .Include(rs => rs.RouteSegment)
                            .ThenInclude(rs => rs.ToStop)
                                .ThenInclude(rs => rs.Locality)
                                    .ThenInclude(rs => rs.UtcTimezone)
                        .Include(rs => rs.RouteSegment)   
                            .ThenInclude(rs => rs.FromStop)
                                .ThenInclude(rs => rs.Locality)
                                    .ThenInclude(rs => rs.UtcTimezone)
                        .Where(rs => rs.RouteScheduleId == trip.RouteScheduleId &&
                                     segmentIds.Contains(rs.RouteSegmentId))
                        .OrderByDescending(rs => rs.Version) 
                        .FirstOrDefaultAsync();

                    if (schedule == null) continue;

                    var tripDto = _mapper.Map<TripDTO>(trip);
                    tripDto.Schedule = _mapper.Map<RouteSegmentScheduleDTO>(schedule);
                    tripDto.Schedule.DepartureDayNumber = await _routeSegmentScheduleService.GetDepartureDayNumber(schedule);
                    tripDto.DepartureDate = tripDto.DepartureDate.AddDays(tripDto.Schedule.DepartureDayNumber.Value);
                    tripsDT0.Add(tripDto);
                }
                else
                {
                    var candidates = await _db.RouteSegmentSchedules
                        .Include(rs => rs.RouteSegment)
                            .ThenInclude(rs => rs.ToStop)
                                .ThenInclude(rs=>rs.Locality)
                                    .ThenInclude(rs => rs.UtcTimezone)
                        .Include(rs => rs.RouteSegment)
                            .ThenInclude(rs => rs.FromStop)
                                .ThenInclude(rs => rs.Locality)
                                    .ThenInclude(rs => rs.UtcTimezone)
                        .Where(rs => rs.RouteScheduleId == trip.RouteScheduleId &&
                                     rs.SegmentNumber.StartsWith("1-"))
                        .ToListAsync(); 

                    schedule = candidates
                        .OrderByDescending(rs => int.Parse(rs.SegmentNumber.Split('-')[1]))
                        .ThenByDescending(rs => rs.Version)
                        .FirstOrDefault();

                    var tripDto = _mapper.Map<TripDTO>(trip);
                    tripDto.Schedule = _mapper.Map<RouteSegmentScheduleDTO>(schedule);
                    tripsDT0.Add(tripDto);
                }
            }

            return tripsDT0;

            //throw new NotImplementedException();
        }
        public async Task<TripDTO> GetTripAsync(int? segmentId, int tripId)
        {
            var trip = await _db.Trips
                .Where(t=>t.Id == tripId)
                .Include(t => t.RouteSchedule)
                    .ThenInclude(rs => rs.Route)
                .Include(t => t.Bus)
                    .ThenInclude(b => b.Seats)
                .Include(t => t.Driver)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Trip with id = {tripId} not found");

            RouteSegmentSchedule? schedule = null;

            // Фильтрация по сегменту маршрута
            if (segmentId.HasValue)
            {
                schedule = await _db.RouteSegmentSchedules
                    .Include(rs => rs.RouteSegment)
                        .ThenInclude(rs => rs.ToStop)
                            .ThenInclude(rs => rs.Locality)
                                .ThenInclude(rs => rs.UtcTimezone)
                    .Include(rs => rs.RouteSegment)
                        .ThenInclude(rs => rs.FromStop)
                            .ThenInclude(rs => rs.Locality)
                                .ThenInclude(rs => rs.UtcTimezone)
                    .Where(rs => rs.RouteScheduleId == trip.RouteScheduleId &&
                                    rs.RouteSegmentId == segmentId)
                    .OrderByDescending(rs => rs.Version)
                    .FirstOrDefaultAsync() ?? 
                    throw new KeyNotFoundException($"RouteSegmentSchedules with id = {segmentId} not found");
            }
            else
            {
                var candidates = await _db.RouteSegmentSchedules
                    .Include(rs => rs.RouteSegment)
                        .ThenInclude(rs => rs.ToStop)
                            .ThenInclude(rs => rs.Locality)
                                .ThenInclude(rs => rs.UtcTimezone)
                    .Include(rs => rs.RouteSegment)
                        .ThenInclude(rs => rs.FromStop)
                            .ThenInclude(rs => rs.Locality)
                                .ThenInclude(rs => rs.UtcTimezone)
                    .Where(rs => rs.RouteScheduleId == trip.RouteScheduleId &&
                                    rs.SegmentNumber.StartsWith("1-"))
                    .ToListAsync();

                schedule = candidates
                    .OrderByDescending(rs => int.Parse(rs.SegmentNumber.Split('-')[1]))
                    .ThenByDescending(rs => rs.Version)
                    .FirstOrDefault() ??
                    throw new KeyNotFoundException($"Parent RouteSegmentSchedule not found");
            }
            var tripDto = _mapper.Map<TripDTO>(trip);
            tripDto.Schedule = _mapper.Map<RouteSegmentScheduleDTO>(schedule);
            tripDto.Schedule.DepartureDayNumber = await _routeSegmentScheduleService.GetDepartureDayNumber(schedule);
            tripDto.DepartureDate = tripDto.DepartureDate.AddDays(tripDto.Schedule.DepartureDayNumber.Value);

            var relatedSegments = await _routeSegmentScheduleService.GetRelatedSegments(schedule);
            var allSeats = await _db.Seats
                .Where(s => s.BusId == trip.Bus.Id)
                .Select(s => new { SeatId = s.Id, SeatNumber = s.SeatNumber })
                .ToListAsync();
            var bookedSeats = await _db.Bookings
                .Where(
                    b => b.TripId == trip.Id &&
                    relatedSegments.Contains(b.RouteSegmentScheduleId) &&
                    !b.BookingStatusHistories
                        .Any(bsh => bsh.Status == BookingStatus.RefundApproved || bsh.Status == BookingStatus.ReserveCancelled) &&
                    (b.Expires == null || b.Expires > DateTimeOffset.UtcNow)
                )
                .Include(b=>b.Seat).Select(b=>b.Seat.SeatNumber).ToListAsync();
            tripDto.Seats = allSeats
                .Select(seat => new SeatDTO
                {
                    Id = seat.SeatId,
                    SeatNumber = seat.SeatNumber,
                    IsAvailable = !bookedSeats.Contains(seat.SeatNumber)
                })
                .ToList();

            return tripDto;
        }
        public async Task CreateAsync(TripPlanRequest planRequest, int carrierId)
        {
            // Валидация дат
            if (planRequest.FromPlanning > planRequest.ToPlanning)
                throw new ArgumentException("FromDate cannot be after ToDate");

            if ((planRequest.ToPlanning.ToDateTime(TimeOnly.MinValue) - 
                planRequest.FromPlanning.ToDateTime(TimeOnly.MinValue)).TotalDays > 60)
                throw new ArgumentException("Date range cannot exceed 60 days");

            var schedule = _db.RouteSchedules
                .Where(rs=>rs.Id==planRequest.RouteScheduleId)
                .Include(rs => rs.Route)
                .Include(rs=>rs.SchedulePattern).FirstOrDefault() ?? 
                throw new KeyNotFoundException($"RouteSchedule with id = {planRequest.RouteScheduleId} not found");
            if (schedule.Route!.CarrierId != carrierId)
                throw new UnauthorizedAccessException("You do not have permission to create trips for this carrier.");
            var schedulePattern = schedule.SchedulePattern ?? 
                throw new KeyNotFoundException($"SchedulePattern not found");

            //Находим пересечение дат
            var validDates = DateHelper.GetValidTripDates(
                schedulePattern.StartDate,
                schedulePattern.EndDate,
                schedulePattern.DaysOfWeek,
                planRequest.FromPlanning,
                planRequest.ToPlanning
            );

            if (!validDates.Any())
                throw new InvalidOperationException("No valid dates found in the specified range");

            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var trips = validDates.Select(date => new Trip
                {
                    RouteScheduleId = planRequest.RouteScheduleId,
                    TripStatus = TripStatus.Scheduled,
                    DepartureDate = date,
                    BusId = planRequest.BusId,
                    DriverId = planRequest.DriverId
                });

                await _db.Trips.AddRangeAsync(trips);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public Task UpdateAsync(TripDTO dto)
        {
            throw new NotImplementedException();
        }
        public async Task<List<TripDTO>> GetDriversTripsAsync(int driverId)
        {
            if (await _db.Drivers.FirstOrDefaultAsync(d => d.Id == driverId) == null)
                throw new KeyNotFoundException($"Driver with id = {driverId} not found");

            var trips = await _db.Trips
                .Include(t => t.RouteSchedule)
                    .ThenInclude(rs => rs.Route)
                .Include(t => t.Bus)
                    .ThenInclude(b => b.Seats)
                .Include(t => t.Driver)
                .OrderBy(t => t.DepartureDate)
                .ThenBy(t => t.RouteSchedule.Route.Number)
                .ToListAsync();

            var tripsDT0 = new List<TripDTO>();
            foreach (var trip in trips)
            {
                RouteSegmentSchedule? schedule = null;
                var candidates = await _db.RouteSegmentSchedules
                    .Include(rs => rs.RouteSegment)
                        .ThenInclude(rs => rs.ToStop)
                            .ThenInclude(rs => rs.Locality)
                                .ThenInclude(rs => rs.UtcTimezone)
                    .Include(rs => rs.RouteSegment)
                        .ThenInclude(rs => rs.FromStop)
                            .ThenInclude(rs => rs.Locality)
                                .ThenInclude(rs => rs.UtcTimezone)
                    .Where(rs => rs.RouteScheduleId == trip.RouteScheduleId &&
                                    rs.SegmentNumber.StartsWith("1-"))
                    .ToListAsync();

                schedule = candidates
                    .OrderByDescending(rs => int.Parse(rs.SegmentNumber.Split('-')[1]))
                    .ThenByDescending(rs => rs.Version)
                    .FirstOrDefault();

                var tripDto = _mapper.Map<TripDTO>(trip);
                tripDto.Schedule = _mapper.Map<RouteSegmentScheduleDTO>(schedule);
                tripsDT0.Add(tripDto);
            }

            return tripsDT0;
        }

        public async Task<TripExecutionDTO> GetTripExecutionAsync(int tripId)
        {
            var tripDto = await GetTripAsync(null, tripId)
                ?? throw new KeyNotFoundException($"Trip with id {tripId} not found");

            // Получаем последовательные сегменты маршрута
            var segments = (await _routeSegmentScheduleService.GetSequentialSegments(tripId))?
                .Where(s => s != null).ToList()
                ?? new List<RouteSegmentScheduleDTO>();

            // Получаем фактические данные выполнения сегментов
            var executions = await _db.TripExecutions
                .Where(te => te.TripId == tripId)
                .ToDictionaryAsync(e => e.RouteSegmentId.ToString());

            // Сопоставляем сегменты с их фактическим выполнением
            var segmentExecutions = segments.Select(segment =>
            {
                executions.TryGetValue(segment.RouteSegment.Id, out var execution);
                var segmentExecutionDto = new TripExecutionSegmentDTO
                {
                    Segment = segment,
                    Departure = execution?.Departure, // будет null если execution == null
                    Arrival = execution?.Arrival      // будет null если execution == null
                };
                if (segmentExecutionDto.Departure != null && segmentExecutionDto.Arrival != null)
                    segmentExecutionDto.Status = SegmentExecutionStatus.Completed.ToString();
                else if(segmentExecutionDto.Departure != null && segmentExecutionDto.Arrival == null)
                    segmentExecutionDto.Status = SegmentExecutionStatus.InProgress.ToString();
                return segmentExecutionDto;
            }).ToList();

            return new TripExecutionDTO
            {
                Trip = tripDto,
                SegmentExecutions = segmentExecutions
            };

        }

        public async Task ConfirmArrivalAsync(int driverId, int tripId, int segmentScheduleId, DateTimeOffset timestamp)
        {
            var tripExecution = await GetTripExecutionAsync(tripId);
            if (tripExecution.Trip.Driver.Id != driverId.ToString())
                throw new UnauthorizedAccessException("wrong driver");
            var routeScheduleIndex = tripExecution.SegmentExecutions.FindIndex(s => s.Segment.Id == segmentScheduleId.ToString());
            if (routeScheduleIndex == -1)
                throw new KeyNotFoundException("wrong segmentScheduleId");

            // Проверяем, что все статусы до routeScheduleIndex равны "completed", а восле "planned"
            bool allCompleted = tripExecution.SegmentExecutions
                .Take(routeScheduleIndex)
                .All(s => s.Status == SegmentExecutionStatus.Completed.ToString());
            bool allPlanned = tripExecution.SegmentExecutions
                .Skip(routeScheduleIndex + 1)
                .All(s => s.Status == SegmentExecutionStatus.Planned.ToString());
            bool currentInProgress = tripExecution.SegmentExecutions[routeScheduleIndex].Status == SegmentExecutionStatus.InProgress.ToString();
            if (!allCompleted || !allPlanned || !currentInProgress)
                throw new ValidationException("wrong trip execution order");

            var trip = await _db.Trips.FirstOrDefaultAsync(t => t.Id == tripId);
            var routeSegmentSchedule = _mapper.Map<RouteSegmentSchedule>(tripExecution.SegmentExecutions[routeScheduleIndex].Segment);
            routeSegmentSchedule.RouteScheduleId = trip!.RouteScheduleId;
            var relatedSegmentIds = await _routeSegmentScheduleService.GetRelatedSegments(routeSegmentSchedule);
            var relatedSegments = await _db.RouteSegmentSchedules.Where(rss => relatedSegmentIds.Contains(rss.Id)).ToListAsync();

            if (!relatedSegments.Any())
                throw new KeyNotFoundException("related segments not found");

            var curSegParts = tripExecution.SegmentExecutions[routeScheduleIndex].Segment.SegmentNumber.Split('-');

            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                if (routeScheduleIndex == tripExecution.SegmentExecutions.Count - 1)
                    trip!.TripStatus = TripStatus.Finished;
                foreach (var rs in relatedSegments)
                {
                    var segParts = rs.SegmentNumber.Split('-');
                    if (segParts[1] != curSegParts[1])
                        continue;

                    var departureRecord = await _db.TripExecutions.FirstOrDefaultAsync(te => te.TripId == tripId && te.RouteSegmentId == rs.RouteSegmentId)
                        ?? throw new ValidationException("wrong trip execution order");
                    departureRecord.Arrival = timestamp;
                }
                await _db.SaveChangesAsync();
                await _ticketService.CheckoutAsync(driverId, new CheckinRequest
                {
                    TripId = tripId,
                    SegmentId = segmentScheduleId
                });
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ConfirmDepartureAsync(int driverId, int tripId, int segmentScheduleId, DateTimeOffset timestamp)
        {
            var tripExecution = await GetTripExecutionAsync(tripId);
            if (tripExecution.Trip.Driver.Id != driverId.ToString())
                throw new UnauthorizedAccessException("wrong driver");
            var routeScheduleIndex = tripExecution.SegmentExecutions.FindIndex(s => s.Segment.Id == segmentScheduleId.ToString());
            if(routeScheduleIndex == -1)
                throw new KeyNotFoundException("wrong segmentScheduleId");

            // Проверяем, что все статусы до routeScheduleIndex равны "completed", а восле "planned"
            bool allCompleted = tripExecution.SegmentExecutions
                .Take(routeScheduleIndex) 
                .All(s => s.Status == SegmentExecutionStatus.Completed.ToString());
            bool allPlanned = tripExecution.SegmentExecutions
                .Skip(routeScheduleIndex)
                .All(s => s.Status == SegmentExecutionStatus.Planned.ToString());
            if (!allCompleted || !allPlanned)
                throw new ValidationException("wrong trip execution order");

            var trip = await _db.Trips.FirstOrDefaultAsync(t => t.Id ==  tripId);
            var routeSegmentSchedule = _mapper.Map<RouteSegmentSchedule>(tripExecution.SegmentExecutions[routeScheduleIndex].Segment);
            routeSegmentSchedule.RouteScheduleId = trip!.RouteScheduleId;
            var relatedSegmentIds = await _routeSegmentScheduleService.GetRelatedSegments(routeSegmentSchedule);
            var relatedSegments = await _db.RouteSegmentSchedules.Where(rss => relatedSegmentIds.Contains(rss.Id)).ToListAsync();

            if(!relatedSegments.Any())
                throw new KeyNotFoundException("related segments not found");

            var curSegParts = tripExecution.SegmentExecutions[routeScheduleIndex].Segment.SegmentNumber.Split('-');

            await using var transaction = await _db.Database.BeginTransactionAsync();
            try {
                if (routeScheduleIndex == 0)
                    trip!.TripStatus = TripStatus.InProgress;
                foreach (var rs in relatedSegments)
                {
                    var segParts = rs.SegmentNumber.Split('-');
                    if (segParts[0] != curSegParts[0])
                        continue;

                    //var departureRecord = await _db.TripExecutions.FirstOrDefaultAsync(te => te.TripId == tripId && te.RouteSegmentId == rs.RouteSegmentId);
                    var departureRecord = new TripExecution
                    {
                        TripId = tripId,
                        RouteSegmentId = rs.RouteSegmentId,
                        Departure = timestamp
                    };
                    _db.TripExecutions.Add(departureRecord);
                }
                await _db.SaveChangesAsync();
                await _ticketService.CheckinAsync(driverId, new CheckinRequest
                {
                    TripId = tripId,
                    SegmentId = segmentScheduleId
                });
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
