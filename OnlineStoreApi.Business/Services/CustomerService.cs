using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Business.Mappers;
using OnlineStoreApi.Data.Models;
using OnlineStoreApi.Data.Repositories;

namespace OnlineStoreApi.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _repository.GetAllAsync();
            return customers.Select(c => c.ToDto());
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");
            return customer.ToDto();
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
        {
            ValidateCustomerInput(dto);

            var existingCustomer = await _repository.GetByEmailAsync(dto.Email);
            if (existingCustomer != null)
                throw new InvalidOperationException("Customer with this email already exists");

            var customer = dto.ToEntity();
            await _repository.AddAsync(customer);
            await _repository.SaveChangesAsync();
            return customer.ToDto();
        }

        public async Task UpdateCustomerAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");

            ValidateCustomerInput(dto);

            var existingCustomer = await _repository.GetByEmailAsync(dto.Email);
            if (existingCustomer != null && existingCustomer.Id != id)
                throw new InvalidOperationException("Another customer with this email already exists");

            dto.UpdateEntity(customer);
            _repository.Update(customer);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");

            _repository.Delete(customer);
            await _repository.SaveChangesAsync();
        }

        private void ValidateCustomerInput(dynamic dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Customer name is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Customer email is required");
        }
    }
}
