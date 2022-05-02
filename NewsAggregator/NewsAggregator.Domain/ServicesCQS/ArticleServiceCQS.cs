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
            return await _mediator.Send(new GetArticleByIdForUserQuery(id), 
                new CancellationToken());
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesForUser()
        {
            return await _mediator.Send(new GetAllArticlesForUserQuery(), 
                new CancellationToken());
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesByPageForUser(int page)
        {
            var size = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

            return await _mediator.Send(new GetArticlesByPageForUserQuery(size, page), 
                new CancellationToken());
        }

        public async Task<ArticleDto> GetArticleByIdForAdmin(Guid id)
        {
            return await _mediator.Send(new GetArticleByIdForAdminQuery(id), 
                new CancellationToken());
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesForAdmin()
        {
            return await _mediator.Send(new GetAllArticlesForAdminQuery(), 
                new CancellationToken());
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesByPageForAdmin(int page)
        {
            var size = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

            return await _mediator.Send(new GetArticlesByPageForAdminQuery(size, page), 
                new CancellationToken());
        }
    }
}
