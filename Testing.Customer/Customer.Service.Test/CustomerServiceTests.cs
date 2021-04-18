using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture.Xunit2;
using Customer.Contract;
using Customer.Domain;
using FluentAssertions;
using Moq;
using Xunit;

namespace Customer.Service.Tests
{
    public class CustomerServiceTests
    {
        [Theory]
        [AutoData]
        public void CreateNewCustomer_ValidParameters_NotThrowException(CustomerDto customerDto,
            Domain.Customer customer)
        {
            var customerMapperMock = new Mock<ICustomerMapper>();
            customerMapperMock.Setup(c => c.ToCustomer(customerDto)).Returns(customer);

            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(c => c.Save(customer)).Returns(It.IsAny<Guid>());

            var customerService = new CustomerService(customerRepositoryMock.Object, customerMapperMock.Object);

            Action act = () => { customerService.CreateNew(customerDto); };

            act.Should().NotThrow<Exception>();
        }

        [Theory]
        [AutoData]
        public void UpdateCustomer_ValidParameters_NotThrowException(CustomerDto customerDto, Domain.Customer customer)
        {
            var customerMapperMock = new Mock<ICustomerMapper>();
            customerMapperMock.Setup(c => c.ToCustomerDto(customer)).Returns(customerDto);

            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(c => c.Get(customerDto.Id)).Returns(customer);

            var customerService = new CustomerService(customerRepositoryMock.Object, customerMapperMock.Object);

            Action act = () =>
            {
                customerService.Update(customerDto);

                customerRepositoryMock.Verify(m => m.Get(customerDto.Id), Times.Once);
                customerRepositoryMock.Verify(m => m.Update(customer), Times.Once);
            };

            act.Should().NotThrow<Exception>();
        }

        [Theory]
        [AutoData]
        public void GetAllCustomers_WhenCalled_ReturnValidCustomerCount(List<CustomerDto> customerDtos,
            List<Domain.Customer> customers)
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(c => c.All()).Returns(customers.AsQueryable);

            var customerMapperMock = new Mock<ICustomerMapper>();
            customerMapperMock.Setup(c => c.ToCustomerDtoList(customers)).Returns(customerDtos);

            var customerService = new CustomerService(customerRepositoryMock.Object, customerMapperMock.Object);

            Action act = () =>
            {
                var result = customerService.GetAll();

                result.Count.Should().Be(customerDtos.Count);
                customerMapperMock.Verify(m => m.ToCustomerDtoList(customers), Times.Once);
            };

            act.Should().NotThrow<Exception>();
        }

        [Theory]
        [AutoData]
        public void GetByCityCode_WhenCalled_ReturnRelatedCustomerDtos(List<CustomerDto> customerDtos,
            List<Domain.Customer> customers, string cityCode)
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(c => c.Find(It.IsAny<Expression<Func<Domain.Customer, bool>>>()))
                .Returns(customers.AsQueryable);

            var customerMapperMock = new Mock<ICustomerMapper>();
            customerMapperMock.Setup(c => c.ToCustomerDtoList(customers)).Returns(customerDtos);

            var customerService = new CustomerService(customerRepositoryMock.Object, customerMapperMock.Object);

            Action act = () =>
            {
                var result = customerService.GetByCityCode(cityCode);
                result.Should().BeEquivalentTo(customerDtos);
            };

            act.Should().NotThrow<Exception>();
        }

        [Theory]
        [AutoData]
        public void GetById_WhenCalled_ReturnRelatedCustomer(CustomerDto customerDto, Domain.Customer customer)
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(c => c.Get(customer.Id)).Returns(customer);

            var customerMapperMock = new Mock<ICustomerMapper>();
            customerMapperMock.Setup(c => c.ToCustomerDto(customer)).Returns(customerDto);

            var customerService = new CustomerService(customerRepositoryMock.Object, customerMapperMock.Object);

            Action act = () =>
            {
                var result = customerService.GetById(customer.Id);
                result.Should().BeEquivalentTo(customerDto);
            };

            act.Should().NotThrow<Exception>();
        }
    }
}