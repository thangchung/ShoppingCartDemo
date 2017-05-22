using System;
using NT.Core.SharedKernel;

namespace NT.Core.OrderContext
{
    public class ShipInfo : ValueObject
    {
        public ShipInfo(Guid id, string name, AddressInfo addressInfo)
        {
            Id = id;
            Name = name;
            AddressInfo = addressInfo;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public AddressInfo AddressInfo { get; private set; }
    }
}