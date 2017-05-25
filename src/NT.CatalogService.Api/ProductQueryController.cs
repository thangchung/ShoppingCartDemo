using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.Core;

namespace NT.CatalogService.Api
{
    [Route("api/products")]
    public class ProductQueryController : Controller
    {
        private readonly IRepository<Product> _genericProductRepository;

        public ProductQueryController(IRepository<Product> genericProductRepository)
        {
            _genericProductRepository = genericProductRepository;
        }

        [HttpGet("{id}")]
        public async Task<Product> Get(Guid id)
        {
            return await _genericProductRepository.GetByIdAsync(id);
        }
    }
}