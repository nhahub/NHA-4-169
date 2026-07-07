using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.Models
{
	public sealed class PaginatedList<T>
	{
		public IReadOnlyList<T> Items { get; }
		public int PageIndex { get; }
		public int TotalPages { get; }
		public int TotalCount { get; }

		public PaginatedList(IReadOnlyList<T> items, int count, int pageIndex, int pageSize)
		{
			Items = items;
			PageIndex = pageIndex;
			TotalCount = count;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
		}

		public bool HasPreviousPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages;
	}

}
