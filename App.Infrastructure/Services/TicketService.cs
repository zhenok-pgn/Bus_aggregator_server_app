using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.Services;

namespace App.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        public Task CheckinAsync(int driverId, CheckinRequest request)
        {
            throw new NotImplementedException();
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
