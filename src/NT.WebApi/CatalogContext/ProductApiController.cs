using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.Infrastructure;

namespace NT.WebApi.CatalogContext
{
    [Route("api/products")]
    public class ProductApiController : BaseGatewayController
    {
        public ProductApiController(RestClient restClient) 
            : base(restClient)
        {
        }

        [HttpGet("{id}")]
        public async Task<Product> Get(Guid id)
        {
            return await RestClient.GetAsync<Product>("catalog_service", $"/api/products/{id}");
        }
    }
}