using Microsoft.EntityFrameworkCore;
using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Data.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbSet.Include(c => c.Orders).ToListAsync();
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _dbSet.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
