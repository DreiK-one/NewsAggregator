using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Queries.CommentQueries;
using NewsAggregetor.CQS.Models.Commands.CommentCommands;
using AutoMapper;
using NewsAggregator.Core.Helpers;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class CommentServiceCQS : ICommentServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentServiceCQS> _logger;
        private readonly IMediator _mediator;

        public CommentServiceCQS(IMapper mapper,
            ILogger<CommentServiceCQS> logger,
            IMediator mediator)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            
        }

        public async Task<CreateOrEditCommentDto> GetByIdAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetCommentByIdQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateOrEditCommentDto dto)
        {
            try
            {
                var command = _mapper.Map<CreateCommentCommand>(dto);

                return await _mediator.Send(command, 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<bool> EditAsync(CreateOrEditCommentDto dto)
        {
            try
            {
                var command = _mapper.Map<EditCommentCommand>(dto);

                return await _mediator.Send(command,
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new DeleteCommentCommand(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        } 
    }
}
