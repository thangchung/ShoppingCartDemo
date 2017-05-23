using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CustomerService.Core;
using NT.Infrastructure;

namespace NT.WebApi.CustomerContext
{
    [Route("api/customers")]
    // [Authorize]
    public class CustomerApiController : Controller
    {
        private readonly RestClient _restClient;

        public CustomerApiController(RestClient restClient)
        {
            _restClient = restClient;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _restClient.GetAsync<List<Customer>>("customer_service", "/api/customers");
        }

        [HttpGet("{id}")]
        public async Task<Customer> Get(Guid id)
        {
            return await _restClient.GetAsync<Customer>("customer_service", $"/api/customers/{id}");
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