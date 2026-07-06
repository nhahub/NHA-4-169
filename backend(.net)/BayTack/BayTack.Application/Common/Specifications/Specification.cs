using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BayTack.Application.Common.Specifications
{
	public abstract class Specification<T>
	{
		public Expression<Func<T, bool>>? Criteria { get; private set; }
		public List<Expression<Func<T, object>>> Includes { get; } = new();
		public List<string> IncludeStrings { get; } = new();

		public Expression<Func<T, object>>? OrderBy { get; private set; }
		public Expression<Func<T, object>>? OrderByDescending { get; private set; }

		public int Skip { get; private set; }
		public int Take { get; private set; }
		public bool IsPagingEnabled { get; private set; }

		public bool AsNoTracking { get; private set; } = true;
		public bool AsSplitQuery { get; private set; }

		protected Specification() { }

		protected Specification(Expression<Func<T, bool>> criteria) => Criteria = criteria;

		protected void AddCriteria(Expression<Func<T, bool>> criteria)
		{
			// Combines with AND if criteria already set, so derived specs can layer filters.
			Criteria = Criteria is null ? criteria : Criteria.AndAlso(criteria);
		}

		protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);
		protected void AddInclude(string includeString) => IncludeStrings.Add(includeString); // For EF Core 5.0+ "ThenInclude" support

		protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;
		protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression) => OrderByDescending = orderByDescExpression;

		protected void ApplyPaging(int pageIndex, int pageSize)
		{
			Skip = (pageIndex - 1) * pageSize;
			Take = pageSize;
			IsPagingEnabled = true;
		}

		protected void EnableTracking() => AsNoTracking = false;
		protected void EnableSplitQuery() => AsSplitQuery = true;
	}

	internal static class ExpressionExtensions
	{
		/// <summary>Combines two predicate expressions with a logical AND, rewriting
		/// parameters so both sides share the same parameter instance.</summary>
		public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
		{
			var parameter = Expression.Parameter(typeof(T));
			var leftVisitor = new ReplaceParameterVisitor(left.Parameters[0], parameter);
			var leftBody = leftVisitor.Visit(left.Body);
			var rightVisitor = new ReplaceParameterVisitor(right.Parameters[0], parameter);
			var rightBody = rightVisitor.Visit(right.Body);
			return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(leftBody!, rightBody!), parameter);
		}

		private class ReplaceParameterVisitor : ExpressionVisitor
		{
			private readonly ParameterExpression _oldParameter;
			private readonly ParameterExpression _newParameter;

			public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
			{
				_oldParameter = oldParameter;
				_newParameter = newParameter;
			}

			protected override Expression VisitParameter(ParameterExpression node) =>
				node == _oldParameter ? _newParameter : base.VisitParameter(node);
		}
	}
}
