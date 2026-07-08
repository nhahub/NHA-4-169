using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Orders.Common;

namespace BayTack.Application.Features.Orders.Queries.GetOrderById
{
	public sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDetailResponse>
	{
		private readonly IOrdersReadRepository _ordersReadRepository;

		public GetOrderByIdQueryHandler(IOrdersReadRepository ordersReadRepository) =>
			_ordersReadRepository = ordersReadRepository;

		public async Task<Result<OrderDetailResponse>> Handle(GetOrderByIdQuery request, CancellationToken ct)
		{
			var order = await _ordersReadRepository.GetByIdForCustomerAsync(request.CustomerId, request.OrderId, ct);

			return order is null
				? Result<OrderDetailResponse>.Failure("Order not found.")
				: Result<OrderDetailResponse>.Success(order);
		}
	}
}
