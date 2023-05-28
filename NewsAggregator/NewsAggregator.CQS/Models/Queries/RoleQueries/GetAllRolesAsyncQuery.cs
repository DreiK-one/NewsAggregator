using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.RoleQueries
{
    public class GetAllRolesAsyncQuery : IRequest<IEnumerable<RoleDto>>
    {
    }
}
