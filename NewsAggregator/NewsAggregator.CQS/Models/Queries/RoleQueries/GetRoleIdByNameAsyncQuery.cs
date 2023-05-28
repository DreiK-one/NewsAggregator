using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.RoleQueries
{
    public class GetRoleIdByNameAsyncQuery : IRequest<Guid>
    {
        public GetRoleIdByNameAsyncQuery(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
