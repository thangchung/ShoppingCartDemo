using System;
using NT.Core;

namespace NT.AuditService.Core
{
    public class AuditInfo : EntityBase
    {
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string ActionMessage { get; set; }
        public DateTime Created { get; set; }
    }
}