using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Core;
using NT.Core.CustomerContext;
using NT.Infrastructure.CustomerContext;

namespace NT.CustomerService
{
    [Route("api/customers")]
    // [Authorize]
    public class CustomerQueryController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IRepository<Customer> _genericCustomerRepository;

        public CustomerQueryController(ICustomerRepository customerRepo, IRepository<Customer> genericCustomerRepository)
        {
            _customerRepo = customerRepo;
            _genericCustomerRepository = genericCustomerRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _genericCustomerRepository.ListAsync(
                new ISpecification<Customer>[]
                {
                    new CustomerWithAddressInfoSpec(),
                    new CustomerWithContactInfoSpec()
                });
        }

        [HttpGet("{id}")]
        public async Task<Customer> Get(Guid id)
        {
            return await _customerRepo.GetFullCustomer(id);
        }
    }
}