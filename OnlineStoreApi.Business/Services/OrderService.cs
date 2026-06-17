using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Business.Mappers;
using OnlineStoreApi.Data.Models;
using OnlineStoreApi.Data.Repositories;

namespace OnlineStoreApi.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(o => o.ToDto());
        }

        /// <summary>
        /// Get order by ID with user authorization check
        /// </summary>
        public async Task<OrderDto> GetOrderByIdAsync(int id, int? userId = null)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            // Enforce user-based access control
            if (userId.HasValue && order.UserId != userId.Value)
                throw new UnauthorizedAccessException("You do not have permission to access this order");

            return order.ToDto();
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId)
        {
            var orders = await _orderRepository.GetByCustomerAsync(customerId);
            return orders.Select(o => o.ToDto());
        }

        /// <summary>
        /// Get all orders for a specific user
        /// </summary>
        public async Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserAsync(userId);
            return orders.Select(o => o.ToDto());
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = dto.ToEntity();
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            return order.ToDto();
        }

        /// <summary>
        /// Create order for a specific user
        /// </summary>
        public async Task<OrderDto> CreateOrderForUserAsync(int userId)
        {
            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = "pending",
                TotalAmount = 0
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            return order.ToDto();
        }

        public async Task AddItemToOrderAsync(int orderId, CreateOrderDetailDto dto, int? userId = null)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found");

            // Enforce user-based access control
            if (userId.HasValue && order.UserId != userId.Value)
                throw new UnauthorizedAccessException("You do not have permission to modify this order");

            if (order.Status == "cancelled")
                throw new InvalidOperationException("Cannot add items to a cancelled order");

            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {dto.ProductId} not found");

            if (product.Stock < dto.Quantity)
                throw new InvalidOperationException($"Insufficient stock. Available: {product.Stock}, Requested: {dto.Quantity}");


            var priceAfterDiscount = product.Price * (1 - product.Discount / 100);
            var orderDetail = new OrderDetail
            {
                OrderId = orderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Price = priceAfterDiscount
            };

            order.OrderDetails.Add(orderDetail);


            product.Stock -= dto.Quantity;


            if (product.Stock == 0)
                product.Status = "tükənib";


            order.TotalAmount = order.OrderDetails.Sum(od => od.Price * od.Quantity);

            _orderRepository.Update(order);
            _productRepository.Update(product);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task RemoveItemFromOrderAsync(int orderId, int orderDetailId, int? userId = null)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found");

            // Enforce user-based access control
            if (userId.HasValue && order.UserId != userId.Value)
                throw new UnauthorizedAccessException("You do not have permission to modify this order");

            var orderDetail = order.OrderDetails.FirstOrDefault(od => od.Id == orderDetailId);
            if (orderDetail == null)
                throw new KeyNotFoundException($"Order detail with ID {orderDetailId} not found");

            if (order.Status == "cancelled")
                throw new InvalidOperationException("Cannot remove items from a cancelled order");


            var product = await _productRepository.GetByIdAsync(orderDetail.ProductId);
            if (product != null)
            {
                product.Stock += orderDetail.Quantity;
                if (product.Status == "tükənib")
                    product.Status = "active";
                _productRepository.Update(product);
            }


            order.OrderDetails.Remove(orderDetail);


            order.TotalAmount = order.OrderDetails.Sum(od => od.Price * od.Quantity);

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task ChangeOrderStatusAsync(int orderId, ChangeOrderStatusDto dto, int? userId = null)
        {
            var validStatuses = new[] { "pending", "confirmed", "cancelled", "delivered" };
            if (!validStatuses.Contains(dto.Status))
                throw new ArgumentException($"Invalid status. Must be one of: {string.Join(", ", validStatuses)}");

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found");

            // Enforce user-based access control
            if (userId.HasValue && order.UserId != userId.Value)
                throw new UnauthorizedAccessException("You do not have permission to modify this order");


            if (dto.Status == "cancelled" && order.Status != "cancelled")
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _productRepository.GetByIdAsync(detail.ProductId);
                    if (product != null)
                    {
                        product.Stock += detail.Quantity;
                        if (product.Status == "tükənib")
                            product.Status = "active";
                        _productRepository.Update(product);
                    }
                }
            }

            order.Status = dto.Status;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id, int? userId = null)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            // Enforce user-based access control
            if (userId.HasValue && order.UserId != userId.Value)
                throw new UnauthorizedAccessException("You do not have permission to delete this order");


            foreach (var detail in order.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(detail.ProductId);
                if (product != null)
                {
                    product.Stock += detail.Quantity;
                    if (product.Status == "tükənib")
                        product.Status = "active";
                    _productRepository.Update(product);
                }
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
