using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data.Entities
{
    public class Source
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string BaseUrl { get; set; }

        public string RssUrl { get; set; }


        public virtual IEnumerable<Article> Articles { get; set; }
    }
}
