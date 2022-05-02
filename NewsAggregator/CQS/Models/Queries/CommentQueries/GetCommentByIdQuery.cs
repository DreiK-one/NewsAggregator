using MediatR;
using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQS.Models.Queries.CommentQueries
{
    public class GetCommentByIdQuery : IRequest<CreateOrEditCommentDto>
    {
        public Guid Id { get; set; }
    }
}
