using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.Data
{
    public interface IArticleRepository
    {
        public IEnumerable<Article> Get5TopRatedNewsOrderedByCreationDate();
    }
}
