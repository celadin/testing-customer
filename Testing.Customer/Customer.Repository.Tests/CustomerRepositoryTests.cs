using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Customer.Repository.Tests
{
    public class CustomerRepositoryTests
    {
        [Fact]
        public void SaveCustomer_ValidParameters_EqualProps()
        {
            var customer = new Domain.Customer("Celalettin Altıntaş", "IST", DateTime.Today.AddYears(-1));

            var options = CreateNewContextOptions();

            Guid newCustomerId;
            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);

                newCustomerId = repository.Save(customer);
                context.SaveChanges();
            }

            Domain.Customer insertedCustomer;
            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);

                insertedCustomer = repository.Get(newCustomerId);
            }

            Assert.Equal(customer.FullName, insertedCustomer.FullName);
            Assert.Equal(customer.CityCode, insertedCustomer.CityCode);
            Assert.Equal(customer.BirthDate, insertedCustomer.BirthDate);
        }

        [Fact]
        public void DeleteCustomer_AddTwoCustomerAndDeleteOne_ReturnCustomersCountAsOne()
        {
            var customer1 = new Domain.Customer("ali", "ist", DateTime.Today.AddYears(-30));
            var customer2 = new Domain.Customer("veli", "ank", DateTime.Today.AddYears(-25));

            var options = CreateNewContextOptions();

            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);
                repository.Save(customer1);
                repository.Save(customer2);

                context.SaveChanges();
            }

            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);
                var customerToBeDelete = repository.All().First();
                repository.Delete(customerToBeDelete.Id);
                context.SaveChanges();
            }

            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);

                var customerCount = repository.All().Count();

                customerCount.Should().Be(1);
            }
        }

        [Fact]
        public void UpdateCustomer_ValidParameters_ValidUpdateProcess()
        {
            var customer = new Domain.Customer("ali", "ist", DateTime.Today.AddYears(-30));

            var options = CreateNewContextOptions();

            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);

                repository.Save(customer);

                context.SaveChanges();
            }

            customer.SetFields("ali updated", "ank", DateTime.Today.AddYears(-20));

            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);

                repository.Update(customer);

                context.SaveChanges();
            }

            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);
                var updatedCustomer = repository.Get(customer.Id);

                updatedCustomer.FullName.Should().Be(customer.FullName);
                updatedCustomer.CityCode.Should().Be(customer.CityCode);
                updatedCustomer.BirthDate.Should().Be(customer.BirthDate);
            }
        }

        [Fact]
        public void FindCustomer_ByCityCode_ReturnValidCustomer()
        {
            var customer1 = new Domain.Customer("ali", "ist", DateTime.Today.AddYears(-30));
            var customer2 = new Domain.Customer("veli", "ank", DateTime.Today.AddYears(-25));

            var options = CreateNewContextOptions();

            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);

                repository.Save(customer1);
                repository.Save(customer2);

                context.SaveChanges();
            }

            using (var context = new CustomerDbContext(options))
            {
                var repository = new CustomerRepository(context);

                var customers = repository.Find(c => c.CityCode == "ist");

                customers.Should().NotBeNull();
                customers.Count().Should().Be(1);
            }
        }

        private static DbContextOptions<CustomerDbContext> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<CustomerDbContext>();
            builder.UseInMemoryDatabase("customer_id")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}