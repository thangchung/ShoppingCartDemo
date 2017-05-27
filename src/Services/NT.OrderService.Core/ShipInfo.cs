using System;
using NT.Core;
using NT.Core.SharedKernel;

namespace NT.OrderService.Core
{
    public class ShipInfo : ValueObject
    {
        internal ShipInfo()
        {
        }

        public ShipInfo(Guid id, string name, AddressInfo addressInfo)
        {
            Id = id;
            Name = name;
            AddressInfo = addressInfo;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public virtual AddressInfo AddressInfo { get; private set; }
    }
}