using System;

namespace NT.Core.UserContext
{
    public class NotificationInfoLink : ValueObject
    {
        public NotificationInfoLink(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
