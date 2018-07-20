using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using vega.Core.Models;

namespace vega.Extentions
{
    public static class IQuerableExtentions
    
    {
        public static IQueryable<T> ApplyOrdering<T>(
            this IQueryable<T> query,
            IQueryObject queryObj,
            Dictionary<string, Expression<Func<T, object>>> columnsMapping)
        {
            if (String.IsNullOrWhiteSpace(queryObj.SortBy) ||!columnsMapping.ContainsKey(queryObj.SortBy))
                return query;
            
            if (queryObj.IsSortAscending)
                return query.OrderBy(columnsMapping[queryObj.SortBy]);
            else
                return query.OrderByDescending(columnsMapping[queryObj.SortBy]);

        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObj)
        {
            if (queryObj.Page <= 0)
                queryObj.Page = 1;
            if (queryObj.PageSize <= 0)
                queryObj.PageSize = 10;
            
            return query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);

        }
    }
}