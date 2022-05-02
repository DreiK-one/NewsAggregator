using CQS.Models.Commands.CommentCommands;
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

        public async Task<bool> CreateAsync(CreateOrEditCommentDto dto)
        {
            return await _mediator.Send(new CreateCommentCommand(dto), new CancellationToken());
        }

    }
}
