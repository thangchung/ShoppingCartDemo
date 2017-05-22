using System;
using System.Linq.Expressions;

namespace NT.Core
{
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Expression<Func<TEntity, object>> Include { get; }
    }
}