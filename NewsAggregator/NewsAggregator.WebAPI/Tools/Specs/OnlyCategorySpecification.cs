using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Specifications;
using System.Linq.Expressions;


namespace NewsAggregator.WebAPI.Tools.Specs
{
    public class OnlyCategorySpecification : Specification<RequestArticleDto>
    {
        private readonly string _categoryName;

        public OnlyCategorySpecification(string category)
        {
            _categoryName = category;
        }
        public override Expression<Func<RequestArticleDto, bool>> ToExpression()
        {
            return request => request.CategoryName.ToUpperInvariant().Equals(_categoryName.ToUpperInvariant());
        }
    }
}
