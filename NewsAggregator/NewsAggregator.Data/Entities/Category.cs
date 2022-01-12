using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Entities
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        public virtual IEnumerable<Article> Articles { get; set; }
    }
}
