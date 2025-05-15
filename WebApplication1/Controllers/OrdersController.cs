using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.Services;
using App.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private IOrderService _orderService { get; set; }
        private ITokenService _tokenService { get; set; }

        public OrdersController(IOrderService orderService, ITokenService tokenService)
        {
            _orderService = orderService;
            _tokenService = tokenService;
        }

        [HttpGet("{order?}")]
        [Authorize(Roles = "Passenger")]
        public async Task<IActionResult> GetOrders(
             OrderNumber? order)
        {
            var buyerId = _tokenService.GetUserIdFromContext();
            if (order == null)
            {
                var trips = await _orderService.GetOrdersAsync(buyerId);
                return Ok(trips);
            }
            else
            {
                var trip = await _orderService.GetOrderAsync(buyerId, order);
                return Ok(trip);
            }
        }

        [HttpPost("pay")]
        [Authorize(Roles = "Passenger")]
        public async Task<IActionResult> PayOrder([FromBody] PayOrderRequest request)
        {
            var buyerId = _tokenService.GetUserIdFromContext();
            await _orderService.PayAsync(buyerId, request);
            return Ok(new { Message = "Successful payed" });
        }

        [HttpPost]
        [Authorize(Roles = "Passenger")]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest createOrderRequest)
        {
            var buyerId = _tokenService.GetUserIdFromContext();
            var order = await _orderService.CreateAsync(buyerId, createOrderRequest);
            return Ok(order);
        }
    }
}
