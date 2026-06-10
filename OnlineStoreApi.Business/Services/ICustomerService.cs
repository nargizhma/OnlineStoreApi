using OnlineStoreApi.Business.DTOs;

namespace OnlineStoreApi.Business.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto);
        Task UpdateCustomerAsync(int id, UpdateCustomerDto dto);
        Task DeleteCustomerAsync(int id);
    }
}
