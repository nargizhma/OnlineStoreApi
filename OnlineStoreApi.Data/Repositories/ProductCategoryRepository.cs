using Microsoft.EntityFrameworkCore;
using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Data.Repositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await _dbSet.Include(pc => pc.Products).FirstOrDefaultAsync(pc => pc.Id == id);
        }

        public override async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await _dbSet.Include(pc => pc.Products).ToListAsync();
        }
    }
}
