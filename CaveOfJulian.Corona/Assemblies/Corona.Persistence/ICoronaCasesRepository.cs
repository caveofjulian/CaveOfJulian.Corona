using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Corona.Persistence
{
    public interface ICoronaCasesRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(string query, params object[] parameters);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        void Insert(IEnumerable<TEntity> entity);
        Task InsertAsync(IEnumerable<TEntity> entity);
        Task SaveAsync();
    }
}