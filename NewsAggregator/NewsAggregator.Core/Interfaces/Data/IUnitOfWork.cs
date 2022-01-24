using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.Data
{
    public interface IUnitOfWork
    {
        object Articles { get; }
        object Roles { get; }
        object Users { get; }
        object Sources { get; }
        object Comments { get; }
        object Categories { get; }

        Task<int> Commit();
    }
}
