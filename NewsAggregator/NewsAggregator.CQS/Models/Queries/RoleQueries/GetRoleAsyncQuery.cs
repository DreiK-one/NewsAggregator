using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.RoleQueries
{
    public class GetRoleAsyncQuery : IRequest<RoleDto>
    {
        public GetRoleAsyncQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
