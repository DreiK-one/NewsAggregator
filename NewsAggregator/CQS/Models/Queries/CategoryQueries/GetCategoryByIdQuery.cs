using MediatR;
using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Queries.CategoryQueries
{
    public class GetCategoryByIdQuery : IRequest<CategoryWithArticlesDto>
    {
        public GetCategoryByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
