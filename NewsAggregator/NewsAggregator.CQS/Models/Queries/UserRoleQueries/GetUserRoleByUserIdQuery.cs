using MediatR;
using NewsAggregator.Data.Entities;


namespace NewsAggregetor.CQS.Models.Queries.UserRoleQueries
{
    public class GetUserRoleByUserIdQuery : IRequest<UserRole>
    {
        public GetUserRoleByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
}
