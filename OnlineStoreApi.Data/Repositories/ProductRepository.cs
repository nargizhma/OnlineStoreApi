using Microsoft.EntityFrameworkCore;
using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await _dbSet.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet.Include(p => p.ProductCategory).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet.Where(p => p.CategoryId == categoryId).Include(p => p.ProductCategory).ToListAsync();
        }
    }
}
