using MediatR;
using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Queries.CategoryQueries
{
    public class GetCategoryByNameQuery : IRequest<CategoryWithArticlesDto>
    {
        public GetCategoryByNameQuery(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
