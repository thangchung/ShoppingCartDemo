using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NT.CustomerService.Core;

namespace NT.CustomerService.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _dbContext;

        public CustomerRepository(CustomerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Customer>> GetFullCustomers()
        {
            return await _dbContext.Set<Customer>()
                .Include(x => x.AddressInfo)
                .Include(x => x.ContactInfo)
                .ToListAsync();
        }

        public async Task<Customer> GetFullCustomer(Guid id)
        {
            return await _dbContext.Set<Customer>()
                .Include(x => x.AddressInfo)
                .Include(x => x.ContactInfo)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}