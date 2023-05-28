using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.RoleQueries
{
    public class GetRoleNameByIdAsyncQuery : IRequest<string>
    {
        public GetRoleNameByIdAsyncQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
