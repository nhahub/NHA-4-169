using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviews
{
	public sealed class ReviewsByOrderIdsSpec : Specification<Review>
	{
		public ReviewsByOrderIdsSpec(List<string> orderIds) : base(r => orderIds.Contains(r.OrderId))
		{
		}
	}
}
