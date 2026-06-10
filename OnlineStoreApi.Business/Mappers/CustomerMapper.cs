using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Data.Models;

namespace OnlineStoreApi.Business.Mappers
{
    public static class CustomerMapper
    {
        public static CustomerDto ToDto(this Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };
        }

        public static Customer ToEntity(this CreateCustomerDto dto)
        {
            return new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };
        }

        public static void UpdateEntity(this UpdateCustomerDto dto, Customer customer)
        {
            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
        }
    }
}
