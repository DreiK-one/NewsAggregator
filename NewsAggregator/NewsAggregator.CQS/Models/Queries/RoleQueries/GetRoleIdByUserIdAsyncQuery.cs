using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.RoleQueries
{
    public class GetRoleIdByUserIdAsyncQuery : IRequest<Guid>
    {
        public GetRoleIdByUserIdAsyncQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
