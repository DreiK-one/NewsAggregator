using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.DataAccess
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
