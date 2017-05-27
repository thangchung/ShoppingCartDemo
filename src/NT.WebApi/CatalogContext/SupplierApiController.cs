using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.Infrastructure.AspNetCore;

namespace NT.WebApi.CatalogContext
{
    [Route("api/suppliers")]
    public class SupplierApiController : BaseGatewayController
    {
        public SupplierApiController(RestClient restClient) 
            : base(restClient)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<Supplier>> Get()
        {
            return await RestClient.GetAsync<List<Supplier>>("catalog_service", "/api/suppliers");
        }

        [HttpGet("{id}")]
        public async Task<Supplier> Get(Guid id)
        {
            return await RestClient.GetAsync<Supplier>("catalog_service", $"/api/suppliers/{id}");
        }
    }
}