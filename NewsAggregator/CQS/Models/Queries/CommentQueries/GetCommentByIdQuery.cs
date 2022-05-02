using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.CommentQueries
{
    public class GetCommentByIdQuery : IRequest<CreateOrEditCommentDto>
    {
        public GetCommentByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
