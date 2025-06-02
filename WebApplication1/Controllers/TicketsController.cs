using App.Application.DTO.Requests;
using App.Application.Services;
using App.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WEB.Controllers
{
    [ApiController]
    [Route("tickets")]
    public class TicketsController : ControllerBase
    {
        private ITicketService _ticketService { get; set; }
        private ITokenService _tokenService { get; set; }

        public TicketsController(ITicketService ticketService, ITokenService tokenService)
        {
            _ticketService = ticketService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> GetTickets(
            [FromQuery] int? tripId)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            var trips = await _ticketService.GetTicketsAsync(carrierId, tripId);
            return Ok(trips);
        }

        [HttpPost("{id}/refund/approve")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> TicketRefundApprove(int id)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            await _ticketService.TicketRefundResponseAsync(carrierId, id, true);
            return Ok(new { Message = "Successful refund approve" });
        }

        [HttpPost("{id}/refund/reject")]
        [Authorize(Roles = "Carrier")]
        public async Task<IActionResult> TicketRefundReject(int id)
        {
            var carrierId = _tokenService.GetUserIdFromContext();
            await _ticketService.TicketRefundResponseAsync(carrierId, id, false);
            return Ok(new { Message = "Successful refund reject" });
        }

        [HttpPost("{id}/refund/request")]
        [Authorize(Roles = "Passenger")]
        public async Task<IActionResult> TicketRefundRequest(int id)
        {
            var buyerId = _tokenService.GetUserIdFromContext();
            await _ticketService.TicketRefundRequestAsync(buyerId, id);
            return Ok(new { Message = "Successful refund request" });
        }

        /*[HttpPost("checkin")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> Checkin([FromBody] CheckinRequest request)
        {
            var driverId = _tokenService.GetUserIdFromContext();
            await _ticketService.CheckinAsync(driverId, request);
            return Ok(new { Message = "Successful create" });
        }*/
    }
}
