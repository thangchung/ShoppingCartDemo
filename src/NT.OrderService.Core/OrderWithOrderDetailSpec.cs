using System;
using System.Linq.Expressions;
using NT.Core;

namespace NT.OrderService.Core
{
    public class OrderWithOrderDetailSpec : ISpecification<Order>
    {
        public Expression<Func<Order, bool>> Criteria
        {
            get { return e => true; }
        }

        public Expression<Func<Order, object>> Include
        {
            get { return e => e.OrderDetails; }
        }
    }
}