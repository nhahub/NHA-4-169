using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Orders.Common;

namespace BayTack.Application.Features.Orders.Queries.GetMyOrders
{
	public sealed class GetMyOrdersQueryHandler : IQueryHandler<GetMyOrdersQuery, List<OrderResponse>>
	{
		private readonly IOrdersReadRepository _ordersReadRepository;

		public GetMyOrdersQueryHandler(IOrdersReadRepository ordersReadRepository) =>
			_ordersReadRepository = ordersReadRepository;

		public async Task<Result<List<OrderResponse>>> Handle(GetMyOrdersQuery request, CancellationToken ct) =>
			await _ordersReadRepository.GetForCustomerAsync(request.CustomerId, request.Status, ct);
	}
}
