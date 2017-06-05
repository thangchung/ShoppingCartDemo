using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.AuditService.Core;
using NT.Infrastructure.AspNetCore;

namespace NT.WebApi.AuditContext
{
    [Route("api/audits")]
    public class AuditApiController : BaseGatewayController
    {
        public AuditApiController(RestClient restClient)
            : base(restClient)
        {
        }

        [HttpGet]
        public async Task<List<AuditInfo>> Get()
        {
            return await RestClient.GetAsync<List<AuditInfo>>("audit_service", "/api/audits");
        }
    }
}