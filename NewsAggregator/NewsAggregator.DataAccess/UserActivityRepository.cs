using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DataAccess
{
    public class UserActivityRepository : BaseRepository<UserActivity>
    {
        public UserActivityRepository(NewsAggregatorContext context) : base(context)
        {

        }
    }
}
