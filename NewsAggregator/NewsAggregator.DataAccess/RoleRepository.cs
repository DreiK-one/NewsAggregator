using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.DataAccess
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
