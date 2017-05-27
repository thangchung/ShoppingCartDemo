using System;
using System.Linq.Expressions;
using NT.Core;

namespace NT.CustomerService.Core
{
    public class CustomerWithContactInfoSpec : ISpecification<Customer>
    {
        public Expression<Func<Customer, bool>> Criteria
        {
            get { return e => true; }
        }

        public Expression<Func<Customer, object>> Include
        {
            get { return e => e.ContactInfo; }
        }
    }
}