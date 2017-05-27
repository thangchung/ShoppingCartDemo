using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NT.CustomerService.Core
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetFullCustomers();
        Task<Customer> GetFullCustomer(Guid id);
    }
}