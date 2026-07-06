using BayTack.Application.Common.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Specification
{
	public static class SpecificationEvaluator<T> where T : class
	{
		public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, Specification<T> spec)
		{
			var query = inputQuery;

			if (spec.Criteria is not null)
				query = query.Where(spec.Criteria);

			query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
			query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

			if (spec.OrderBy is not null)
				query = query.OrderBy(spec.OrderBy);
			else if (spec.OrderByDescending is not null)
				query = query.OrderByDescending(spec.OrderByDescending);

			if (spec.IsPagingEnabled)
				query = query.Skip(spec.Skip).Take(spec.Take);

			if (spec.AsNoTracking)
				query = query.AsNoTracking();

			if (spec.AsSplitQuery)
				query = query.AsSplitQuery();

			return query;
		}
	}

}
