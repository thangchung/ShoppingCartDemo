using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.Core;

namespace NT.CatalogService.Api
{
    [Route("api/suppliers")]
    public class SupplierApiController : Controller
    {
        private readonly IRepository<Supplier> _genericSupplierRepository;

        public SupplierApiController(IRepository<Supplier> genericSupplierRepository)
        {
            _genericSupplierRepository = genericSupplierRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Supplier>> Get()
        {
            return await _genericSupplierRepository.ListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Supplier> Get(Guid id)
        {
            return await _genericSupplierRepository.GetByIdAsync(id);
        }
    }
}