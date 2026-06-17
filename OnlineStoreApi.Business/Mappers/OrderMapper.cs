using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Business.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Date = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CustomerId = order.CustomerId ?? 0,
                CustomerName = order.Customer?.Name,
                OrderDetails = order.OrderDetails?.Select(od => od.ToDto()).ToList() ?? new List<OrderDetailDto>()
            };
        }

        public static OrderDetailDto ToDto(this OrderDetail detail)
        {
            return new OrderDetailDto
            {
                Id = detail.Id,
                Quantity = detail.Quantity,
                Price = detail.Price,
                ProductId = detail.ProductId,
                ProductName = detail.Product?.Name
            };
        }

        public static Order ToEntity(this CreateOrderDto dto)
        {
            return new Order
            {
                CustomerId = dto.CustomerId,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 0
            };
        }
    }
}
