using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Roni.Corona.Persistence
{
    public class CoronaCasesRepository<TEntity> : ICoronaCasesRepository<TEntity> where TEntity:class
    {
        private readonly RoniContext _context;
        private readonly DbSet<TEntity> _cases;

        public CoronaCasesRepository(RoniContext context)
        {
            _context = context;
            _cases = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get(string query, params object[] parameters)
        {
            return _cases.FromSqlRaw(query, parameters);
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _cases;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return orderBy != null ? orderBy(query) : query;
        }

        public virtual IQueryable<TEntity> Get()
        {
            return _cases;
        }

        public virtual void Insert(IEnumerable<TEntity> entity)
        {
            _cases.AddRange(entity);
        }

        public virtual async Task InsertAsync(IEnumerable<TEntity> entity)
        {
            await _cases.AddRangeAsync(entity);
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
