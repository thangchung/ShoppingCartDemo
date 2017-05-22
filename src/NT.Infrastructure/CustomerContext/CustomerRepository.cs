using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NT.Core.CustomerContext;

namespace NT.Infrastructure.CustomerContext
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
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

    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetFullCustomers();
        Task<Customer> GetFullCustomer(Guid id);
    }
}