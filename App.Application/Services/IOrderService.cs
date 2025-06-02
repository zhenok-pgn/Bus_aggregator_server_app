using App.Application.DTO;
using App.Application.DTO.Requests;

namespace App.Application.Services
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetOrdersAsync(int buyerId);
        Task<OrderDTO> GetOrderAsync(OrderNumber order);
        Task PayAsync(int buyerId, PayOrderRequest request);
        Task<OrderNumber> CreateAsync(int buyerId, CreateOrderRequest request);
    }
}
