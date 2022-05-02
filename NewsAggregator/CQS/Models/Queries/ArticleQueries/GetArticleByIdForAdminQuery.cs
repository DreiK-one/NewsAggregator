using MediatR;
using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Queries.ArticleQueries
{
    public class GetArticleByIdForAdminQuery : IRequest<ArticleDto>
    {
        public Guid Id { get; set; }

        public GetArticleByIdForAdminQuery(Guid id)
        {
            Id = id;
        }
        
    }
}
