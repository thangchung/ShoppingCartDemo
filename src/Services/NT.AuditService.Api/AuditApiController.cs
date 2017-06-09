using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.AuditService.Core;
using NT.Core;
using NT.Core.Results;
using NT.Infrastructure;

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
            var result = await _genericAuditRepository.ListAsync();
            return result.OrderByDescending(x => x.Created);
        }

        [HttpPost("{serviceName}/{methodName}/{actionMessage}")]
        public async Task<SagaResult> Post(string serviceName, string methodName, string actionMessage)
        {
            var auditInfo = await _genericAuditRepository.AddAsync(new AuditInfo
            {
                ServiceName = serviceName,
                MethodName = methodName,
                ActionMessage = actionMessage,
                Created = DateTime.UtcNow
            });
            if (auditInfo == null)
                return await Task.FromResult(new SagaResult {Succeed = false});

            return await Task.FromResult(new SagaResult {Succeed = true});
        }
    }
}