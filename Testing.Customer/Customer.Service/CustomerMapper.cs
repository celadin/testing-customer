using System.Collections.Generic;
using Customer.Contract;

namespace Customer.Service
{
    public class CustomerMapper : ICustomerMapper
    {
        public Domain.Customer ToCustomer(CustomerDto customerDto)
        {
            return new(customerDto.FullName, customerDto.CityCode, customerDto.BirthDate);
        }

        public CustomerDto ToCustomerDto(Domain.Customer customer)
        {
            return new()
            {
                Id = customer.Id,
                BirthDate = customer.BirthDate,
                CityCode = customer.CityCode,
                FullName = customer.FullName
            };
        }

        public List<CustomerDto> ToCustomerDtoList(List<Domain.Customer> customerList)
        {
            var response = new List<CustomerDto>();
            foreach (var customer in customerList)
            {
                var customerDto = ToCustomerDto(customer);
                response.Add(customerDto);
            }

            return response;
        }
    }
}