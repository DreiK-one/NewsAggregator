using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DataAccess
{
    public class SourceRepository : BaseRepository<Source>
    {
        public SourceRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}
