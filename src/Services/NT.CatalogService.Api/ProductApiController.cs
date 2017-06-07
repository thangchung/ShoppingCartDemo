using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.Core;
using NT.Core.Results;

namespace NT.CatalogService.Api
{
    [Route("api/products")]
    public class ProductApiController : Controller
    {
        private readonly IRepository<Product> _genericProductRepository;

        public ProductApiController(IRepository<Product> genericProductRepository)
        {
            _genericProductRepository = genericProductRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            var result =  await _genericProductRepository.ListAsync();
            return result.OrderByDescending(x => x.Name);
        }

        [HttpGet("{id}")]
        public async Task<Product> Get(Guid id)
        {
            return await _genericProductRepository.GetByIdAsync(id);
        }

        [HttpGet("{productId}/price")]
        public async Task<SagaDoubleResult> GetPrice(Guid productId)
        {
            var product = await _genericProductRepository.GetByIdAsync(productId);
            if (product == null)
                return await Task.FromResult(new SagaDoubleResult { Succeed = false });
            return await Task.FromResult(new SagaDoubleResult { Succeed = true, Price = product.Price });
        }

        [HttpPut("{productId}/increase-quantity/{quantityInOrder}")]
        public async Task<SagaResult> IncreaseQuantityInCatalog(Guid productId, int quantityInOrder)
        {
            var product = await _genericProductRepository.GetByIdAsync(productId);
            if (product == null && product.Quantity < 0)
                return await Task.FromResult(new SagaResult { Succeed = false });

            product.Quantity += quantityInOrder;
            await _genericProductRepository.UpdateAsync(product);

            return await Task.FromResult(new SagaResult { Succeed = true });
        }

        [HttpPut("{productId}/decrease-quantity/{quantityInOrder}")]
        public async Task<SagaResult> DecreaseQuantityInCatalog(Guid productId, int quantityInOrder)
        {
            var product = await _genericProductRepository.GetByIdAsync(productId);
            if (product == null || product.Quantity < quantityInOrder)
                return await Task.FromResult(new SagaResult {Succeed = false});

            product.Quantity -= quantityInOrder;
            await _genericProductRepository.UpdateAsync(product);

            return await Task.FromResult(new SagaResult {Succeed = true});
        }
    }
}