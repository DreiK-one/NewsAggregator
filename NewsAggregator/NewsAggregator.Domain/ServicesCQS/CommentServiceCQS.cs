using CQS.Models.Commands.CommentCommands;
using CQS.Models.Queries.CommentQueries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;

namespace NewsAggregator.Domain.ServicesCQS
{
    public class CommentServiceCQS : ICommentServiceCQS
    {
        private readonly ILogger<CommentServiceCQS> _logger;
        private readonly IMediator _mediator;

        public CommentServiceCQS(ILogger<CommentServiceCQS> logger, 
            IMediator mediator)
        {
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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateOrEditCommentDto dto)
        {
            try
            {
                return await _mediator.Send(new CreateCommentCommand(dto), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> EditAsync(CreateOrEditCommentDto dto)
        {
            try
            {
                return await _mediator.Send(new EditCommentCommand(dto), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
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
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        } 
    }
}
