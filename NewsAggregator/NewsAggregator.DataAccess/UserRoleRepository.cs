using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.DataAccess
{
    public class UserRoleRepository : BaseRepository<UserRole>
    {
        public UserRoleRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
