using NewsAggregator.Core.Interfaces.Data;

namespace NewsAggregator.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        public object Articles => throw new NotImplementedException();

        public object Roles => throw new NotImplementedException();

        public object Users => throw new NotImplementedException();

        public object Sources => throw new NotImplementedException();

        public object Comments => throw new NotImplementedException();

        public object Categories => throw new NotImplementedException();

        public Task<int> Commit()
        {
            throw new NotImplementedException();
        }
    }
}