using System;

namespace NT.Core
{
    [Serializable]
    public class Event : IEvent
    {
        public byte[] Version { get; set; }
    }
}