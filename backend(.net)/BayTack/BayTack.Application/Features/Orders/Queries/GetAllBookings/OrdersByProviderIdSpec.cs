using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Orders.Queries.GetAllBookings
{
	public sealed class OrdersByProviderIdSpec : Specification<Order>
	{
		public OrdersByProviderIdSpec(string providerId, string? status)
			: base(BuildCriteria(providerId, status))
		{
		}

		private static System.Linq.Expressions.Expression<Func<Order, bool>> BuildCriteria(string providerId, string? status)
		{
			if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
				return o => o.ProviderId == providerId && o.Status == parsedStatus;

			return o => o.ProviderId == providerId;
		}
	}
}
