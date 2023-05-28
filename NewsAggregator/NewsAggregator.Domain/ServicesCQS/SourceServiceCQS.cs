using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Queries.SourceQueries;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class SourceServiceCQS : ISourceServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SourceServiceCQS> _logger;
        private readonly IMediator _mediator;

        public SourceServiceCQS(IMapper mapper,
            ILogger<SourceServiceCQS> logger,
            IMediator mediator)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IEnumerable<SourceDto>> GetAllSourcesAsync()
        {
            try
            {
                return await _mediator.Send(new GetAllSourcesAsyncQuery(), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<SourceDto> GetSourceAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetSourceAsyncQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<Guid> GetSourceByUrl(string url)
        {
            try
            {
                return await _mediator.Send(new GetSourceByUrlQuery(url), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<IEnumerable<RssUrlsFromSourceDto>> GetRssUrlsAsync()
        {
            try
            {
                return await _mediator.Send(new GetRssUrlsAsyncQuery(), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> CreateAsync(SourceDto sourceDto)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> UpdateAsync(SourceDto sourceDto)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        } 
    }
}
