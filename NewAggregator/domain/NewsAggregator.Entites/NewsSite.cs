using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Entites
{
    internal class NewsSite
    {
        public int Id { get; set; }

        public string? Source { get; set; }

        public virtual IEnumerable<News>? News { get; set; }
    }
}
