using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Entites
{
    internal class News
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string? Url { get; set; }

        public float Coefficient { get; set; }

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public int NewsSiteId { get; set; }

        public virtual NewsSite? NewsSite { get; set; }

        public virtual IEnumerable<Comment>? Comments { get; set; }

        public virtual IEnumerable<UserActivity>? UserActivities { get; set; }
    }
}
