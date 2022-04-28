using NewsAggregator.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.Data
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        public Task Add(T obj);
        public Task AddRange(IEnumerable<T> obj);

        public Task<T> GetById(Guid id);
        public Task<T> GetByIdWithIncludes(Guid id, params Expression<Func<T, object>>[] includes);
        public IQueryable<T> Get();

        public Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] includes);

        public Task Update(T obj);

        public Task PatchAsync(Guid id, List<PatchModel> patchDtos);

        public Task Remove(Guid id);

        public Task RemoveRange(Expression<Func<T, bool>> predicate);

        public void Dispose();
    }
}
