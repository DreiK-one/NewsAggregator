using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Commands.ArticleCommands;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class ArticleServiceCQS : IArticleServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleServiceCQS> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public ArticleServiceCQS(IMapper mapper, 
            ILogger<ArticleServiceCQS> logger, 
            IMediator mediator, 
            IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticles(int? page, string? role)
        {
            try
            {
                role = role ?? Variables.Roles.Anonimous;
                if (page > 0 && page != null)
                {
                    return await GetArticlesByPage(Convert.ToInt32(page), role);
                }

                if (role.Equals(Variables.Roles.Admin))
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

        public async Task<int?> CreateAsync(CreateOrEditArticleDto articleDto)
        {
            try
            {
                var command = _mapper.Map<CreateArticleCommand>(articleDto);

                return await _mediator.Send(command, 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> UpdateAsync(CreateOrEditArticleDto articleDto)
        {
            try
            {
                var command = _mapper.Map<EditArticleCommand>(articleDto);

                return await _mediator.Send(command,
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> DeleteAsync(Guid modelId)
        {
            try
            {
                return await _mediator.Send(new DeleteArticleCommand(modelId),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<ArticleDto> GetArticleWithAllNavigationProperties(Guid id)
        {
            try
            {
                return _mediator.Send(new GetArticleWithAllNavigationPropertiesQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<ArticleDto> GetArticleWithAllNavigationPropertiesByRating(Guid id)
        {
            try
            {
                return _mediator.Send(new GetArticleWithAllNavigationPropertiesByRatingQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<List<string>> GetAllExistingArticleUrls()
        {
            try
            {
                return _mediator.Send(new GetAllExistingArticleUrlsQuery(), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<ArticleDto> GetArticleWithoutRating()
        {
            try
            {
                return _mediator.Send(new GetArticleWithoutRatingQuery(), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<IEnumerable<ArticleDto>> GetAllNewsByRatingAsync()
        {
            try
            {
                return _mediator.Send(new GetAllNewsByRatingAsyncQuery(), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<IEnumerable<ArticleDto>> GetNewsByRatingByPageAsync(int page)
        {
            try
            {
                return _mediator.Send(new GetNewsByRatingByPageAsyncQuery(page), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<ArticleDto> MostRatedArticleByPeriodOfTime(float? maxCoef)
        {
            try
            {
                return _mediator.Send(new MostRatedArticleByPeriodOfTimeQuery(maxCoef), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<float?> MaxCoefOfToday()
        {
            try
            {
                return _mediator.Send(new MaxCoefOfTodayQuery(),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<float?> MaxCoefOfTheMonth()
        {
            try
            {
                return _mediator.Send(new MaxCoefOfTheMonthQuery(),
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<float?> MaxCoefOfAllTime()
        {
            try
            {
                return _mediator.Send(new MaxCoefOfAllTimeQuery(), 
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
                    _configuration[Variables.ConfigurationFields.PageSize]);

                if (role.Equals(Variables.Roles.Admin))
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
