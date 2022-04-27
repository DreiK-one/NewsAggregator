using System.Linq.Expressions;

namespace NewsAggregator.WebAPI.Tools.Specifications
{
    public abstract class LinqSpecification<T> : CompositeSpecification<T>
    {
        public abstract Expression<Func<T, bool>> AsExpression();
        public override bool IsSatisfiedBy(T article) => AsExpression().Compile()(article);
    }
}
