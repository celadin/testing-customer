using System;
using AutoFixture.Xunit2;
using Customer.Contract;
using FluentAssertions;
using Xunit;

namespace Customer.Service.Tests
{
    public class CustomerMapperTests
    {
        [Theory]
        [AutoData]
        public void ToCustomer_ValidParameters_NotThrowException(CustomerDto customerDto)
        {
            var customerMapper = new CustomerMapper();

            Action act = () =>
            {
                var customer = customerMapper.ToCustomer(customerDto);

                customer.FullName.Should().Be(customerDto.FullName);
                customer.CityCode.Should().Be(customerDto.CityCode);
                customer.BirthDate.Should().Be(customerDto.BirthDate);
            };

            act.Should().NotThrow<Exception>();
        }

        [Theory]
        [AutoData]
        public void ToCustomerDto_ValidParameters_NotThrowException(Domain.Customer customer)
        {
            var customerMapper = new CustomerMapper();

            Action act = () =>
            {
                var customerDto = customerMapper.ToCustomerDto(customer);

                customerDto.FullName.Should().Be(customer.FullName);
                customerDto.CityCode.Should().Be(customer.CityCode);
                customerDto.BirthDate.Should().Be(customer.BirthDate);
            };

            act.Should().NotThrow<Exception>();
        }
    }
}