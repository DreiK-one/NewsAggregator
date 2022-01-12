using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Entities
{
    public class Article
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public DateTime CreationDate { get; set; }

        public string Url { get; set; }

        public float Coefficient { get; set; }


        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }


        public Guid SourceId { get; set; }

        public virtual Source Source { get; set; }


        public virtual IEnumerable<Comment> Comments { get; set; }

        public virtual IEnumerable<UserActivity> UserActivities { get; set; }
    }
}
