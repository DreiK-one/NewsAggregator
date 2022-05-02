using CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;

namespace NewsAggregator.Domain.ServicesCQS
{
    public class ArticleServiceCQS : IArticleServiceCQS
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public ArticleServiceCQS(IMediator mediator, 
            IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<ArticleDto> GetArticleByIdForUser(Guid id)
        {
            var query = new GetArticleByIdForUserQuery { Id = id};
            var article = await _mediator.Send(query, new CancellationToken());

            return article;
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesForUser()
        {
            var query = new GetAllArticlesForUserQuery();
            var articles = await _mediator.Send(query, new CancellationToken());

            return articles;
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesByPageForUser(int page)
        {
            var query = new GetArticlesByPageForUserQuery
            {
                PageSize = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]),
                PageNumber = page
            };
            var articles = await _mediator.Send(query, new CancellationToken());

            return articles;
        }

        public async Task<ArticleDto> GetArticleByIdForAdmin(Guid id)
        {
            var query = new GetArticleByIdForUserQuery { Id = id };
            var article = await _mediator.Send(query, new CancellationToken());

            return article;
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesForAdmin()
        {
            var query = new GetAllArticlesForAdminQuery();
            var articles = await _mediator.Send(query, new CancellationToken());

            return articles;
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesByPageForAdmin(int page)
        {
            var query = new GetArticlesByPageForAdminQuery
            {
                PageSize = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]),
                PageNumber = page
            };
            var articles = await _mediator.Send(query, new CancellationToken());

            return articles;
        }
    }
}
