using MediatR;
using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Commands.CommentCommands
{
    public class CreateCommentCommand : IRequest<bool>
    {
        public CreateCommentCommand(CreateOrEditCommentDto comment)
        {
            Comment = comment;
        }

        public CreateOrEditCommentDto Comment { get; set; }
    }
}
