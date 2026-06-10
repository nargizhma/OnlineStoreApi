namespace OnlineStoreApi.Business.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }

    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
    }

    public class ChangeOrderStatusDto
    {
        public string? Status { get; set; }
    }
}
