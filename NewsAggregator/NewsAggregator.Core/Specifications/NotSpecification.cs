using NewsAggregator.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Specifications
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        readonly ISpecification<T> _other;
        public NotSpecification(ISpecification<T> other) => _other = other;
        public override bool IsSatisfiedBy(T candidate) => !_other.IsSatisfiedBy(candidate);
    }
}
