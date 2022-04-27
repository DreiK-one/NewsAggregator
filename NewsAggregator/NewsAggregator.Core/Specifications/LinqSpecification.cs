using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Specifications
{
    public abstract class LinqSpecification<T> : CompositeSpecification<T>
    {
        public abstract Expression<Func<T, bool>> AsExpression();
        public override bool IsSatisfiedBy(T candidate) => AsExpression().Compile()(candidate);
    }
}
