using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.Services;
using App.Core.Entities;
using App.Core.Enums;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace App.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        public readonly ApplicationDBContext _db;
        public TicketService(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task CheckinAsync(int driverId, CheckinRequest request)
        {
            var bookings = await _db.Bookings
                .Where(b => b.TripId == request.TripId && b.RouteSegmentScheduleId == request.SegmentId)
                .Include(b => b.BookingStatusHistories)
                .Where(b => b.BookingStatusHistories
                            .OrderByDescending(h => h.StatusChangedAt)
                            .FirstOrDefault().Status == BookingStatus.Paid)
                .ToListAsync();
            if (!bookings.Any())
                return;

            // Добавляем новый статус для каждого бронирования
            for (var i = 0; i < bookings.Count; i++)
            {
                bookings[i].BookingStatusHistories.Add(new BookingStatusHistory
                {
                    Status = BookingStatus.Confirmed,
                    StatusChangedAt = DateTimeOffset.UtcNow,
                });
            }
            await _db.SaveChangesAsync();
        }

        public async Task CheckoutAsync(int driverId, CheckinRequest request)
        {
            var bookings = await _db.Bookings
                .Where(b => b.TripId == request.TripId && b.RouteSegmentScheduleId == request.SegmentId)
                .Include(b => b.BookingStatusHistories)
                .Where(b => b.BookingStatusHistories
                            .OrderByDescending(h => h.StatusChangedAt)
                            .FirstOrDefault().Status == BookingStatus.Confirmed)
                .ToListAsync();
            if (!bookings.Any())
                return;

            // Добавляем новый статус для каждого бронирования
            for (var i = 0; i < bookings.Count; i++)
            {
                bookings[i].BookingStatusHistories.Add(new BookingStatusHistory
                {
                    Status = BookingStatus.Completed,
                    StatusChangedAt = DateTimeOffset.UtcNow,
                });
            }
            await _db.SaveChangesAsync();
        }

        public Task<List<TicketDTO>> GetTicketsAsync(int carrierId, int? tripId)
        {
            throw new NotImplementedException();
        }

        public Task TicketRefundRequestAsync(int buyerId, int ticketId)
        {
            throw new NotImplementedException();
        }

        public Task TicketRefundResponseAsync(int carrierId, int ticketId, bool isApproved)
        {
            throw new NotImplementedException();
        }
    }
}
