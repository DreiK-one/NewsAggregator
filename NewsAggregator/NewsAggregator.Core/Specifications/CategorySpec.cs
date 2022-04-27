using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Specifications
{
    public class CategorySpec : CompositeSpecification<RequestArticleDto>
    {
        private readonly RequestArticleDto _dto;

        public CategorySpec(RequestArticleDto dto)
        {
            _dto = dto;
        }

        public override bool IsSatisfiedBy(RequestArticleDto candidate)
        {
            return _dto.CategoryName.Equals(candidate.CategoryName);
        }
    }
}
