using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.DataAccess
{
    public class CommentRepository : BaseRepository<Comment>
    {
        public CommentRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
