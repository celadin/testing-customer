using Microsoft.EntityFrameworkCore;

namespace Customer.Repository
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Customer> Customers { get; set; }
    }
}