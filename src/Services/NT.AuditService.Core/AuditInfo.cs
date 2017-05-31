using System;
using NT.Core;

namespace NT.AuditService.Core
{
    public enum ActionType
    {
        Query,
        Create,
        Modify,
        Delete
    }

    public class AuditInfo : EntityBase
    {
        public ActionType ActionType { get; set; }
        public string ActionMessage { get; set; }
        public string Source { get; set; }
        public DateTime Created { get; set; }
    }
}