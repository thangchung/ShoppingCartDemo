using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.AuditService.Core;
using NT.Core;
using NT.Core.Results;

namespace NT.AuditService.Api
{
    [Route("api/audits")]
    public class AuditApiController : Controller
    {
        private readonly IRepository<AuditInfo> _genericAuditRepository;

        public AuditApiController(IRepository<AuditInfo> genericAuditRepository)
        {
            _genericAuditRepository = genericAuditRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<AuditInfo>> Get()
        {
            return await _genericAuditRepository.ListAsync();
        }

        [HttpPost("{type}/{source}/{actionMessage}")]
        public async Task<SagaResult> Post(ActionType type, string source, string actionMessage)
        {
            var auditInfo = await _genericAuditRepository.AddAsync(new AuditInfo
            {
                ActionType = type,
                ActionMessage = actionMessage,
                Source = source,
                Created = DateTimeOffset.Now.UtcDateTime
            });
            if (auditInfo == null)
                return await Task.FromResult(new SagaResult {Succeed = false});

            return await Task.FromResult(new SagaResult {Succeed = true});
        }
    }
}