using NewsAggregator.WebAPI.Tools.Interfaces;

namespace NewsAggregator.WebAPI.Tools.Specifications
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        readonly ISpecification<T> _other;
        public NotSpecification(ISpecification<T> other) => _other = other;
        public override bool IsSatisfiedBy(T article) => !_other.IsSatisfiedBy(article);
    }
}
