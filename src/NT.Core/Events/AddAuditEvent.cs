namespace NT.Core.Events
{
    public class AddAuditEvent : Event
    {
        public AddAuditEvent(string serviceName, string methodName, string actionMessage)
        {
            ServiceName = serviceName;
            MethodName = methodName;
            ActionMessage = actionMessage;
        }

        public string ServiceName { get; }
        public string MethodName { get; }
        public string ActionMessage { get; }
    }
}