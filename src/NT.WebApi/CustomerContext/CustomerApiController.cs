using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NT.Core;
using NT.Core.CustomerContext;
using NT.Infrastructure.CustomerContext;

namespace NT.WebApi.CustomerContext
{
    [Route("api/customers")]
    [Authorize]
    public class CustomerApiController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IRepository<Customer> _genericCustomerRepository;

        public CustomerApiController(IRepository<Customer> genericCustomerRepository, ICustomerRepository customerRepo)
        {
            _genericCustomerRepository = genericCustomerRepository;
            _customerRepo = customerRepo;
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

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}