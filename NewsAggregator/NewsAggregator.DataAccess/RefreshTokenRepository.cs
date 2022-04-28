using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DataAccess
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>
    {
        public RefreshTokenRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
