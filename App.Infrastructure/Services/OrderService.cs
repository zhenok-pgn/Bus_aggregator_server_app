using App.Application.DTO;
using App.Application.DTO.Requests;
using App.Application.Services;

namespace App.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        public Task<OrderNumber> CreateAsync(int buyerId, CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO> GetOrderAsync(int buyerId, OrderNumber order)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDTO>> GetOrdersAsync(int buyerId)
        {
            throw new NotImplementedException();
        }

        public Task PayAsync(int buyerId, PayOrderRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
