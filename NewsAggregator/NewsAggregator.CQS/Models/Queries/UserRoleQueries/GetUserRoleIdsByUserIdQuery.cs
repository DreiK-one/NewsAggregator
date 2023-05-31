using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.UserRoleQueries
{
    public class GetUserRoleIdsByUserIdQuery : IRequest<IEnumerable<Guid>>
    {
        public GetUserRoleIdsByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
}
