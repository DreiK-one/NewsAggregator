using NewsAggregator.Data;
using NewsAggregator.Data.Entities;


namespace NewsAggregator.DataAccess
{
    public class SourceRepository : BaseRepository<Source>
    {
        public SourceRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
