using System;
using System.ComponentModel.Design;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Customer.Domain.Tests
{
    public class CustomerTests
    {
        [Theory]
        [AutoData]
        public void NewCustomer_FullNameIsEmpty_ThrowException(string cityCode, DateTime birthDate)
        {
            Assert.Throws<ArgumentException>(() => new Customer(string.Empty, cityCode, birthDate));
        }

        [Theory]
        [AutoData]
        public void NewCustomer_CityCodeIsEmpty_ThrowException(string fullName, DateTime birthDate)
        {
            Assert.Throws<ArgumentException>(() => new Customer(fullName, string.Empty, birthDate));
        }

        [Theory]
        [AutoData]
        public void NewCustomer_BirthDateIsToday_ThrowException(string fullName, string cityCode)
        {
            Assert.Throws<ArgumentException>(() => new Customer(fullName, cityCode, DateTime.Today));
        }

        [Theory]
        [AutoData]
        public void NewCustomer_ValidProperties_Success(string fullName, string cityCode, DateTime birthDate)
        {
            var customer = new Customer(fullName, cityCode, birthDate);

            customer.FullName.Should().Be(fullName);
            customer.CityCode.Should().Be(cityCode);
            customer.BirthDate.Should().Be(birthDate);
        }
    }
}