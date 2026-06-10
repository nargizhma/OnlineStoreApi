using OnlineStoreApi.Business.DTOs;

namespace OnlineStoreApi.Business.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task AddItemToOrderAsync(int orderId, CreateOrderDetailDto dto);
        Task RemoveItemFromOrderAsync(int orderId, int orderDetailId);
        Task ChangeOrderStatusAsync(int orderId, ChangeOrderStatusDto dto);
        Task DeleteOrderAsync(int id);
    }
}
