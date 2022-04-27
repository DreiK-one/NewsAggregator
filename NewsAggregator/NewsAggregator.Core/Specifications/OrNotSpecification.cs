using NewsAggregator.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Specifications
{
    public class OrNotSpecification<T> : CompositeSpecification<T>
    {
        readonly ISpecification<T> _left;
        readonly ISpecification<T> _right;

        public OrNotSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) || !_right.IsSatisfiedBy(candidate);
    }
}
