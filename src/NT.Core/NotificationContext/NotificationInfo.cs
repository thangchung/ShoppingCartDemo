using System;

namespace NT.Core.NotificationContext
{
    public class NotificationInfo : ValueObject
    {
        public NotificationInfo(Guid id, string message)
        {
            Id = id;
            Message = message;
        }

        public Guid Id { get; private set; }
        public string Message { get; private set; }

        public override string ToString()
        {
            return $"Notification [id={Id}; message={Message}]";
        }
    }
}