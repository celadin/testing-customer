namespace Customer.Repository
{
    public class CustomerRepository : GenericRepository<Domain.Customer>
    {
        public CustomerRepository(CustomerDbContext dbContext) : base(dbContext)
        {
        }
    }
}