using System.Collections.Generic;
using Customer.Contract;

namespace Customer.Service
{
    public interface ICustomerMapper
    {
        Domain.Customer ToCustomer(CustomerDto customerDto);
        CustomerDto ToCustomerDto(Domain.Customer customer);
        List<CustomerDto> ToCustomerDtoList(List<Domain.Customer> customerList);
    }
}