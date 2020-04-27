﻿using Microsoft.EntityFrameworkCore;
using System;
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
    }
}