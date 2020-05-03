using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Shared.Infrastructure.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DbContext dataContext;
        private DbSet<T> table;

        public RepositoryBase(DbContext dataContext)
        {
            this.dataContext = dataContext;
            table = dataContext.Set<T>();
        }
        public void Create(T entity)
        {
            this.table.Add(entity);
        }

        public void Delete(T entity)
        {
            this.table.Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return this.table.AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.table.Where(expression).AsNoTracking();
        }

        public void Update(T entity)
        {
            this.table.Update(entity);
        }

       public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       string includeProperties = "")
        {
            IQueryable<T> query = table;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }
    }
}
