using System;
using System.Collections.Generic;
using System.Linq;
using Customer.Contract;
using Customer.Domain;

namespace Customer.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerMapper _customerMapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository, ICustomerMapper customerMapper)
        {
            _customerRepository = customerRepository;
            _customerMapper = customerMapper;
        }

        public void CreateNew(CustomerDto customerDto)
        {
            var customer = _customerMapper.ToCustomer(customerDto);
            _customerRepository.Save(customer);
        }

        public CustomerDto Update(CustomerDto customerDto)
        {
            var existingCustomer = _customerRepository.Get(customerDto.Id);
            existingCustomer.SetFields(customerDto.FullName, customerDto.CityCode, customerDto.BirthDate);

            _customerRepository.Update(existingCustomer);

            return _customerMapper.ToCustomerDto(existingCustomer);
        }

        public List<CustomerDto> GetAll()
        {
            return _customerMapper.ToCustomerDtoList(_customerRepository.All().ToList());
        }

        public List<CustomerDto> GetByCityCode(string cityCode)
        {
            var customers = _customerRepository.Find(c => c.CityCode == cityCode).ToList();
            return _customerMapper.ToCustomerDtoList(customers);
        }

        public CustomerDto GetById(Guid id)
        {
            var customer = _customerRepository.Get(id);
            if (customer == null)
                throw new Exception("Customer with this id : " + id + " not found.");
            return _customerMapper.ToCustomerDto(customer);
        }
    }
}