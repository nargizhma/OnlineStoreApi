using OnlineStoreApi.Business.DTOs;

namespace OnlineStoreApi.Business.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id, int? userId = null);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(int userId);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDto> CreateOrderForUserAsync(int userId);
        Task AddItemToOrderAsync(int orderId, CreateOrderDetailDto dto, int? userId = null);
        Task RemoveItemFromOrderAsync(int orderId, int orderDetailId, int? userId = null);
        Task ChangeOrderStatusAsync(int orderId, ChangeOrderStatusDto dto, int? userId = null);
        Task DeleteOrderAsync(int id, int? userId = null);
    }
}
