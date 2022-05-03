using MediatR;
using NewsAggregator.Core.DTOs;


namespace NewsAggregetor.CQS.Models.Queries.AccountQueries
{
    public class GetRoleIdByRoleNameQuery : IRequest<Guid>
    {
        public GetRoleIdByRoleNameQuery(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
