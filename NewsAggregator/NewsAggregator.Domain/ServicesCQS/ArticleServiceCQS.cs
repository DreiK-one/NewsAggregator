using CQS.Models.Queries.ArticleQueries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;

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

        public async Task<ArticleDto> GetArticleByIdForUser(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetArticleByIdForUserQuery(id),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesForUser()
        {
            try
            {
                return await _mediator.Send(new GetAllArticlesForUserQuery(),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            } 
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesByPageForUser(int page)
        {
            try
            {
                var size = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                return await _mediator.Send(new GetArticlesByPageForUserQuery(size, page),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ArticleDto> GetArticleByIdForAdmin(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetArticleByIdForAdminQuery(id),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }          
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesForAdmin()
        {
            try
            {
                return await _mediator.Send(new GetAllArticlesForAdminQuery(),
                                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }            
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesByPageForAdmin(int page)
        {
            try
            {
                var size = Convert.ToInt32(
                    _configuration["ApplicationVariables:PageSize"]);

                return await _mediator.Send(new GetArticlesByPageForAdminQuery(size, page),
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
