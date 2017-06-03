using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CustomerService.Core;
using NT.Infrastructure.AspNetCore;

namespace NT.WebApi.CustomerContext
{
    [Route("api/customers")]
    public class CustomerApiController : BaseGatewayController
    {
        public CustomerApiController(RestClient restClient) 
            : base(restClient)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await RestClient.GetAsync<List<Customer>>("customer_service", "/api/customers");
        }

        [HttpGet("{id}")]
        public async Task<Customer> Get(Guid id)
        {
            return await RestClient.GetAsync<Customer>("customer_service", $"/api/customers/{id}");
        }

        [HttpPost]
        public async Task<Customer> Post([FromBody] Customer customer)
        {
            return await RestClient.PostAsync<Customer>("customer_service", "/api/customers", customer);
        }

        [HttpPatch]
        public void Put()
        {

        }

        [HttpDelete]
        public void Delete()
        {
            
        }
    }
}