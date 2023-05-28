using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Commands.SourceCommands;
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

        public async Task<int?> CreateAsync(SourceDto sourceDto)
        {
            try
            {
                if (sourceDto != null)
                {
                    var existSources = GetAllSourcesAsync().Result
                        .Select(c => c.Name.ToLower() == sourceDto.Name.ToLower());

                    if (!existSources.Any())
                    {
                        var command = _mapper.Map<CreateSourceCommand>(sourceDto);

                        return await _mediator.Send(command,
                            new CancellationToken());
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> UpdateAsync(SourceDto sourceDto)
        {
            try
            {
                if (sourceDto != null)
                {
                    var command = _mapper.Map<UpdateSourceCommand>(sourceDto);

                    return await _mediator.Send(command,
                        new CancellationToken());
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                if (GetSourceAsync(id) != null)
                {
                    return await _mediator.Send(new DeleteSourceCommand(id),
                        new CancellationToken());
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        } 
    }
}
