using MediatR;
using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Commands.CommentCommands
{
    public class DeleteCommentCommand : IRequest<bool>
    {
        public DeleteCommentCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
