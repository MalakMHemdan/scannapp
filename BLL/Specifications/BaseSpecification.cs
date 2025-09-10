using System;
using System.Linq.Expressions;

public abstract class BaseSpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
}
