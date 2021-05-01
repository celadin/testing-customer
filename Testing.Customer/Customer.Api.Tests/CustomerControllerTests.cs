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

        [Theory]
        [AutoData]
        public void Post_WhenCalled_ReturnExpectedStatusCode(CustomerDto customerDto)
        {
            var customerServiceMock = new Mock<ICustomerService>();

            var customerController = new CustomerController(customerServiceMock.Object);

            var result = customerController.Post(customerDto);

            var apiOkResult = result.Should().BeOfType<OkResult>().Subject;
            apiOkResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            customerServiceMock.Verify(c => c.CreateNew(customerDto));
        }

        [Theory]
        [AutoData]
        public void Put_WhenCalled_ReturnCustomer(CustomerDto customerDto)
        {
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(c => c.Update(customerDto)).Returns(customerDto);

            var customerController = new CustomerController(customerServiceMock.Object);

            var result = customerController.Put(customerDto);

            var apiOkResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            apiOkResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedCustomer = apiOkResult.Value.Should().BeAssignableTo<CustomerDto>().Subject;

            returnedCustomer.Should().Be(customerDto);
        }

        [Theory]
        [AutoData]
        public void GetByCityCode_WhenCalled_ReturnListOfCustomer(string cityCode, List<CustomerDto> expectedCustomers)
        {
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(c => c.GetByCityCode(cityCode)).Returns(expectedCustomers);

            var customerController = new CustomerController(customerServiceMock.Object);

            var result = customerController.GetByCityCode(cityCode);

            var apiOkResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            apiOkResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedCustomers = apiOkResult.Value.Should().BeAssignableTo<List<CustomerDto>>().Subject;

            returnedCustomers.Should().BeEquivalentTo(expectedCustomers);
        }
    }
}