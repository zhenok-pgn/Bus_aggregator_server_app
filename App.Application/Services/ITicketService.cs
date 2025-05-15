using App.Application.DTO;
using App.Application.DTO.Requests;

namespace App.Application.Services
{
    public interface ITicketService
    {
        Task<List<TicketDTO>> GetTicketsAsync(int carrierId, int? tripId);
        Task TicketRefundResponseAsync(int carrierId, int ticketId, bool isApproved);
        Task TicketRefundRequestAsync(int buyerId, int ticketId);
        Task CheckinAsync(int driverId, CheckinRequest request);
    }
}
