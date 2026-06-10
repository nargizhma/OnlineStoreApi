using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Data.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);
    }
}
