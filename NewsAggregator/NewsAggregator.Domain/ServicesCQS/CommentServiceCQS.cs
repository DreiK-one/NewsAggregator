using CQS.Models.Commands.CommentCommands;
using CQS.Models.Queries.CommentQueries;
using MediatR;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.InterfacesCQS;

namespace NewsAggregator.Domain.ServicesCQS
{
    public class CommentServiceCQS : ICommentServiceCQS
    {
        private readonly IMediator _mediator;

        public CommentServiceCQS(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateOrEditCommentDto> GetByIdAsync(Guid id)
        {
            return await _mediator.Send(new GetCommentByIdQuery { Id = id }, new CancellationToken());
        }

        public async Task<bool> CreateAsync(CreateOrEditCommentDto dto)
        {
            return await _mediator.Send(new CreateCommentCommand(dto), new CancellationToken());
        }

        public async Task<bool> EditAsync(CreateOrEditCommentDto dto)
        {
            return await _mediator.Send(new EditCommentCommand(dto), new CancellationToken());
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        } 
    }
}
