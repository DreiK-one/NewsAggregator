using MediatR;
using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Queries.ArticleQueries
{
    public class GetArticlesByPageForUserQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetArticlesByPageForUserQuery(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }    
    }
}
