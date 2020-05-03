using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Shared.Infrastructure.SortingHelper
{
    public class Sorter<T> : ISorter<T>
	{
		public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString)
		{
			if (!entities.Any())
				return entities;

			if (string.IsNullOrWhiteSpace(orderByQueryString))
			{
				return entities;
			}

			var orderParams = orderByQueryString.Trim().Split(',');
			var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var orderQueryBuilder = new StringBuilder();

			foreach (var param in orderParams)
			{
				if (string.IsNullOrWhiteSpace(param))
					continue;

				var propertyFromQueryName = param.Split(' ')[0];
				var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
														 StringComparison.InvariantCultureIgnoreCase));

				if (objectProperty == null)
					continue;

				var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

				orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
			}

			if (string.IsNullOrEmpty(orderQueryBuilder.ToString()))
			{
				throw new Exception($"Property Name  wasn't found on {typeof(T)}");
			}

			var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

			return entities.OrderBy(orderQuery);
		}
	}
}
