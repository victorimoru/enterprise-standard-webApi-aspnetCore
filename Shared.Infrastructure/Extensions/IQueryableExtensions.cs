using Shared.Infrastructure.PagingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Shared.Infrastructure.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, ISortCriteria criterion,

              Dictionary<string, Expression<Func<T, object>>> columnMap)
        {
            if (string.IsNullOrEmpty(criterion.SortBy)) return query;
            if (!columnMap.ContainsKey(criterion.SortBy)) return query;
            if(criterion.IsSortAscending)
            {
                query = query.OrderBy(columnMap[criterion.SortBy]);
            }
            else
            {
                query = query.OrderByDescending(columnMap[criterion.SortBy]);
            }

            return query;
        }

        public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, QueryStringParameters parameters,
             params Expression<Func<T, bool>>[] filters) 
        {
            if (filters == null) return query;

            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }

            return query;
        }
    }



    public interface ISortCriteria
    {
        bool IsSortAscending { get; set; }
        string SortBy { get; set; }

    }
}
