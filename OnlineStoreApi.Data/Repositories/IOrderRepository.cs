using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
    }
}
