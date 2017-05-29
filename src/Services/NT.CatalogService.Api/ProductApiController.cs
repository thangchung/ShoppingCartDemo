using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.Core;

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

        [HttpGet("{id}")]
        public async Task<Product> Get(Guid id)
        {
            return await _genericProductRepository.GetByIdAsync(id);
        }

        [HttpPut("{productId}/increase-quantity/{quantityInOrder}")]
        public async Task<SagaResult> IncreaseQuantityInCatalog(Guid productId, int quantityInOrder)
        {
            var product = await _genericProductRepository.GetByIdAsync(productId);
            if (product == null)
                return await Task.FromResult(new SagaResult { Succeed = false });

            product.Quantity += quantityInOrder;
            await _genericProductRepository.UpdateAsync(product);

            return await Task.FromResult(new SagaResult { Succeed = true });
        }

        [HttpPut("{productId}/descrease-quantity/{quantityInOrder}")]
        public async Task<SagaResult> DescreaseQuantityInCatalog(Guid productId, int quantityInOrder)
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