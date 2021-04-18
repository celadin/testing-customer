using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using Customer.Api.Controllers;
using Customer.Contract;
using Customer.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Customer.Api.Tests
{
    public class CustomerControllerTests
    {
        [Theory]
        [AutoData]
        public void GetAll_WhenCalled_ReturnExpectedCustomers(List<CustomerDto> expectedCustomers)
        {
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(c => c.GetAll()).Returns(expectedCustomers);

            var customerController = new CustomerController(customerServiceMock.Object);

            var result = customerController.Get();

            var apiOkResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            apiOkResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var actual = apiOkResult.Value.Should().BeAssignableTo<List<CustomerDto>>().Subject;

            actual.Should().BeEquivalentTo(expectedCustomers);
        }

        [Theory]
        [AutoData]
        public void GetById_WhenCalled_ReturnExpectedCustomer(CustomerDto customerDto, Guid customerId)
        {
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(c => c.GetById(customerId)).Returns(customerDto);

            var customerController = new CustomerController(customerServiceMock.Object);

            var result = customerController.Get(customerId);

            var apiOkResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            apiOkResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var actual = apiOkResult.Value.Should().BeAssignableTo<CustomerDto>().Subject;

            actual.Should().BeEquivalentTo(customerDto);
        }
    }
}