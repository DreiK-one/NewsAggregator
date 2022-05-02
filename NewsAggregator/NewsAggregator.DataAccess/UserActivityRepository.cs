using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.DataAccess
{
    public class UserActivityRepository : BaseRepository<UserActivity>
    {
        public UserActivityRepository(NewsAggregatorContext context) : base(context)
        {

        }
    }
}
