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
        private readonly ApplicationDBContext _db;
        private readonly ITripNotifier _tripNotifier;

        public TicketService(ApplicationDBContext db, ITripNotifier tripNotifier)
        {
            _db = db;
            _tripNotifier = tripNotifier;
        }

        public async Task CheckinAsync(int driverId, CheckinRequest request)
        {
            var segmentNumber = await _db.RouteSegmentSchedules.Where(rss => rss.Id == request.SegmentId).Select(rss => rss.SegmentNumber).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("Сегмент маршрута не найден при посадке на рейс");
            var segmentParts = segmentNumber.Split('-');
            var bookings = await _db.Bookings
                .Where(b => b.TripId == request.TripId && b.RouteSegmentSchedule.SegmentNumber.StartsWith(segmentParts[0] + '-'))
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
                // Уведомляем о посадке на рейс
                await _tripNotifier.SendRouteSegmentStatusUpdateAsync(bookings[i].RouteSegmentScheduleId, BookingStatus.Confirmed.ToString());
            }
            await _db.SaveChangesAsync();
        }

        public async Task CheckoutAsync(int driverId, CheckinRequest request)
        {
            var segmentNumber = await _db.RouteSegmentSchedules.Where(rss => rss.Id == request.SegmentId).Select(rss => rss.SegmentNumber).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("Сегмент маршрута не найден при высадке");
            var segmentParts = segmentNumber.Split('-');
            var bookings = await _db.Bookings
                .Where(b => b.TripId == request.TripId && b.RouteSegmentSchedule.SegmentNumber.EndsWith('-' + segmentParts[1]))
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
                // Уведомляем о высадке с рейса
                await _tripNotifier.SendRouteSegmentStatusUpdateAsync(bookings[i].RouteSegmentScheduleId, BookingStatus.Completed.ToString());
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
