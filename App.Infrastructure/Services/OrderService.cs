using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.Services;
using App.Core.Entities;
using App.Core.Enums;
using App.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace App.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly IRouteSegmentScheduleService _routeSegmentScheduleService;

        public OrderService(ApplicationDBContext db, IMapper mapper, IRouteSegmentScheduleService routeSegmentScheduleService)
        {
            _db = db;
            _mapper = mapper;
            _routeSegmentScheduleService = routeSegmentScheduleService;
        }

        public async Task<OrderNumber> CreateAsync(int buyerId, CreateOrderRequest request)
        {
            var segment = await _db.RouteSegmentSchedules.FirstOrDefaultAsync(s => s.Id == request.SegmentId) ?? 
                throw new KeyNotFoundException($"RouteSegmentSchedules with id={request.SegmentId} while creating order not found");
            // Проверка доступности мест
            var relatedSegments = await _routeSegmentScheduleService.GetRelatedSegments(segment);
            var bookedSeats = await _db.Bookings
                .Where(b => b.TripId == request.TripId &&
                           relatedSegments.Contains(b.RouteSegmentScheduleId) &&
                           request.SeatIds.Contains(b.SeatId) &&
                           !b.BookingStatusHistories.Any(bsh =>
                               bsh.Status == BookingStatus.ReserveCancelled ||
                               bsh.Status == BookingStatus.RefundApproved))
                .Select(b => b.SeatId)
                .ToListAsync();

            if (bookedSeats.Any())
            {
                throw new InvalidOperationException(
                    $"Seats {string.Join(", ", bookedSeats)} already booked");
            }

            var createdAt = DateTimeOffset.UtcNow;
            foreach (var seatId in request.SeatIds)
            {
                var booking = new Booking
                {
                    BuyerId = buyerId,
                    TripId = request.TripId,
                    RouteSegmentScheduleId = request.SegmentId,
                    SeatId = seatId,
                    OrderCreated = createdAt,
                    Expires = createdAt.AddMinutes(30),
                    BookingStatusHistories = new List<BookingStatusHistory>
                    {
                        new BookingStatusHistory
                        {
                            Status = BookingStatus.Reserved,
                            StatusChangedAt = createdAt
                        }
                    }
                };
                _db.Bookings.Add(booking);
            }
            await _db.SaveChangesAsync();
            return new OrderNumber { CreatedAt = createdAt, UserId = buyerId };
        }

        public async Task<OrderDTO> GetOrderAsync(OrderNumber order)
        {
            var orderData = await _db.Bookings
                .Where(b => b.BuyerId == order.UserId && b.OrderCreated == order.CreatedAt)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.RouteSchedule)
                        .ThenInclude(rs => rs.Route)
                .Include(b => b.RouteSegmentSchedule)
                    .ThenInclude(rs => rs.RouteSegment)
                            .ThenInclude(rs => rs.ToStop)
                                .ThenInclude(rs => rs.Locality)
                .Include(b => b.RouteSegmentSchedule)
                    .ThenInclude(rs => rs.RouteSegment)
                            .ThenInclude(rs => rs.FromStop)
                                .ThenInclude(rs => rs.Locality)
                .Include(b => b.Seat)
                .Include(b => b.BookingStatusHistories)
                .Select(b => new
                {
                    Booking = b,
                    LastStatus = b.BookingStatusHistories
                        .OrderByDescending(h => h.StatusChangedAt)
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (!orderData.Any())
                throw new KeyNotFoundException("Order not found");

            // Группировка данных в OrderDTO
            var orderDto = new OrderDTO
            {
                OrderNumber = order,
                Trip = _mapper.Map<TripDTO>(orderData.First().Booking.Trip),
                RouteSegmentSchedule = _mapper.Map<RouteSegmentScheduleDTO>(
                    orderData.First().Booking.RouteSegmentSchedule),
                Bookings = new List<BookingDTO>()
            };
            orderDto.RouteSegmentSchedule.DepartureDayNumber = await _routeSegmentScheduleService.GetDepartureDayNumber(orderData.First().Booking.RouteSegmentSchedule!);
            orderDto.Trip.DepartureDate = orderDto.Trip.DepartureDate.AddDays(orderDto.RouteSegmentSchedule.DepartureDayNumber.Value);
            foreach (var b in orderData)
            {
                var ticket = await _db.Tickets.Where(t => t.BookingId == b.Booking.Id).Include(t => t.Passenger).FirstOrDefaultAsync();
                orderDto.Bookings.Add(new BookingDTO
                {
                    Id = b.Booking.Id.ToString(),
                    SeatNumber = b.Booking.Seat.SeatNumber,
                    BookingStatus = b.LastStatus?.Status.ToString() ?? "Unknown",
                    Ticket = ticket != null ? _mapper.Map<TicketDTO>(ticket) : null
                });
            }
            return orderDto;
        }

        public async Task<List<OrderDTO>> GetOrdersAsync(int buyerId)
        {
            var bookingIds = await _db.Bookings
                .Where(b => b.BuyerId == buyerId)
                .Select(b => b.Id)
                .ToListAsync();

            var ticketsDict = await _db.Tickets
                .Where(t => bookingIds.Contains(t.BookingId))
                .ToDictionaryAsync(t => t.BookingId);

            var orders = await _db.Bookings
                .Where(b => b.BuyerId == buyerId)
                .Include(b => b.Trip)
                .Include(b => b.RouteSegmentSchedule)
                    .ThenInclude(rs => rs.RouteSegment)
                            .ThenInclude(rs => rs.ToStop)
                                .ThenInclude(rs => rs.Locality)
                .Include(b => b.RouteSegmentSchedule)
                    .ThenInclude(rs => rs.RouteSegment)
                            .ThenInclude(rs => rs.FromStop)
                                .ThenInclude(rs => rs.Locality)
                .Include(b => b.Seat)
                .Include(b => b.BookingStatusHistories)
                .AsSplitQuery()
                .GroupBy(b => new { b.OrderCreated, b.TripId, b.RouteSegmentScheduleId })
                .Select(g => new
                {
                    OrderCreated = g.Key.OrderCreated,
                    Trip = g.First().Trip,
                    RouteSegmentSchedule = g.First().RouteSegmentSchedule,
                    Bookings = g.Select(b => new
                    {
                        Booking = b,
                        LastStatus = b.BookingStatusHistories
                            .OrderByDescending(h => h.StatusChangedAt)
                            .FirstOrDefault(),
                        Ticket = ticketsDict.GetValueOrDefault(b.Id)
                    }).ToList()
                })
                .ToListAsync();

            return orders.Select(o => new OrderDTO
            {
                OrderNumber = new OrderNumber
                {
                    UserId = buyerId,
                    CreatedAt = o.OrderCreated
                },
                Trip = _mapper.Map<TripDTO>(o.Trip),
                RouteSegmentSchedule = _mapper.Map<RouteSegmentScheduleDTO>(o.RouteSegmentSchedule),
                Bookings = o.Bookings.Select(b => new BookingDTO
                {
                    Id = b.Booking.Id.ToString(),
                    SeatNumber = b.Booking.Seat.SeatNumber,
                    BookingStatus = b.LastStatus?.Status.ToString() ?? "Unknown",
                    Ticket = b.Ticket != null ? _mapper.Map<TicketDTO>(b.Ticket) : null
                }).ToList()
            }).ToList();
        }

        public async Task PayAsync(int buyerId, PayOrderRequest request)
        {
            var bookings = await _db.Bookings
                .Where(b=>b.BuyerId == request.OrderNumber.UserId && b.OrderCreated == request.OrderNumber.CreatedAt)
                .Include(b => b.BookingStatusHistories)
                .Include(b => b.RouteSegmentSchedule)
                .Where(b => !(b.Expires < DateTimeOffset.UtcNow ||
                           b.BookingStatusHistories
                            .OrderByDescending(h => h.StatusChangedAt)
                            .FirstOrDefault().Status == BookingStatus.ReserveCancelled))
                .ToListAsync();
            if (bookings == null || bookings.Count == 0 || bookings.Count != request.Passengers.Count)
                throw new ValidationException("Order not found or expired");

            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Добавляем новый статус для каждого бронирования
                for (var i = 0; i < bookings.Count; i++)
                {
                    bookings[i].BookingStatusHistories.Add(new BookingStatusHistory
                    {
                        Status = BookingStatus.Paid,
                        StatusChangedAt = DateTimeOffset.UtcNow,
                    });

                    var ticket = new Ticket
                    {
                        BookingId = bookings[i].Id,
                        Passenger = _mapper.Map<Passenger>(request.Passengers[i]),
                        Price = bookings[i].RouteSegmentSchedule.Price,
                        CreatedAt = DateTimeOffset.UtcNow
                    };
                    bookings[i].Expires = null;
                    _db.Tickets.Add(ticket);
                }
                await _db.SaveChangesAsync();
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
