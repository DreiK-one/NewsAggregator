using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }          
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticles()
        {
            try
            {
                return await _mediator.Send(new GetAllArticlesQuery(),
                                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }            
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesByPage(int page)
        {
            try
            {
                var size = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                return await _mediator.Send(new GetArticlesByPageQuery(size, page),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetPositiveArticlesByPage(int page)
        {
            try
            {
                var size = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                return await _mediator.Send(new GetPositiveArticlesByPageQuery(size, page),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
