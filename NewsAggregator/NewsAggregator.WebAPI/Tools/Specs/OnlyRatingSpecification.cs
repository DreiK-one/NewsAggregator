using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Specifications;
using System.Linq.Expressions;

namespace NewsAggregator.WebAPI.Tools.Specs
{
    public class OnlyRatingSpecification : Specification<RequestArticleDto>
    {
        private readonly float _rating;

        public OnlyRatingSpecification(float rating)
        {
            _rating = rating;
        }
        public override Expression<Func<RequestArticleDto, bool>> ToExpression()
        {
            return request => request.Rating.Equals(_rating);
        }
    }
}
