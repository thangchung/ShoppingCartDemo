namespace NT.Core.NotificationContext
{
    public class Notification : EntityBase
    {
        public NotificationInfo NotificationInfo { get; set; }
        public OrderLink Order { get; set; }
    }
}
