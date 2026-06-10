using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    }
}
