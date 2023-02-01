using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class ArticleServiceCQS : IArticleServiceCQS
    {
        private readonly ILogger<ArticleServiceCQS> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public ArticleServiceCQS(ILogger<ArticleServiceCQS> logger, 
            IMediator mediator, 
            IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<ArticleDto> GetArticleById(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetArticleByIdQuery(id),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }          
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticles(int? page, string? role)
        {
            try
            {
                role = role ?? "Anonimous";
                if (page > 0 && page != null)
                {
                   return await GetArticlesByPage(Convert.ToInt32(page), role);
                }

                if (role.Equals("Admin"))
                {
                    return await _mediator.Send(new GetAllArticlesQuery(),
                    new CancellationToken());
                }

                return await _mediator.Send(new GetAllPositiveArticlesQuery(),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }            
        }

        private async Task<IEnumerable<ArticleDto>> GetArticlesByPage(int page, string role)
        {
            try
            {
                var size = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                if (role.Equals("Admin"))
                {
                    return await _mediator.Send(new GetArticlesByPageQuery(size, page),
                        new CancellationToken());
                }

                return await _mediator.Send(new GetPositiveArticlesByPageQuery(size, page));
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
