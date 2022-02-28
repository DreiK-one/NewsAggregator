using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface IArticlesConcurrentService
    {
        Task<int?> GetNewsFromSources();
    }
}
