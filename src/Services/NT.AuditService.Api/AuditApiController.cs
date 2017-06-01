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

        [HttpPost("{serviceName}/{methodName}/{actionMessage}")]
        public async Task<SagaResult> Post(string serviceName, string methodName, string actionMessage)
        {
            var auditInfo = await _genericAuditRepository.AddAsync(new AuditInfo
            {
                ServiceName = serviceName,
                MethodName = methodName,
                ActionMessage = actionMessage,
                Created = DateTimeOffset.Now.UtcDateTime
            });
            if (auditInfo == null)
                return await Task.FromResult(new SagaResult {Succeed = false});

            return await Task.FromResult(new SagaResult {Succeed = true});
        }
    }
}