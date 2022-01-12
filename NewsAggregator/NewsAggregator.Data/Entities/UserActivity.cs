using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Entities
{
    public class UserActivity
    {
        public Guid Id { get; set; }

        public int NumberOfViews { get; set; }

        public DateTime ViewingDate { get; set; }


        public Guid UserId { get; set; }

        public virtual User User { get; set; }


        public Guid ArticleId { get; set; }

        public virtual Article Article { get; set; }
    }
}
