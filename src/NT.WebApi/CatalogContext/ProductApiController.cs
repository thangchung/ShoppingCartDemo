using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.Infrastructure.AspNetCore;

namespace NT.WebApi.CatalogContext
{
    [Route("api/products")]
    public class ProductApiController : BaseGatewayController
    {
        public ProductApiController(RestClient restClient) 
            : base(restClient)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await RestClient.GetAsync<List<Product>>("catalog_service", "/api/products");
        }

        [HttpGet("{id}")]
        public async Task<Product> Get(Guid id)
        {
            return await RestClient.GetAsync<Product>("catalog_service", $"/api/products/{id}");
        }
    }
}