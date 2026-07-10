using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.OrderAggregate;

namespace BayTack.Application.Features.Orders.Queries.GetAllBookings
{
	public sealed class GetAllBookingsQueryHandler : IQueryHandler<GetAllBookingsQuery, List<BookingResponse>>
	{
		private readonly IRepository<Order, string> _orderRepository;

		public GetAllBookingsQueryHandler(IRepository<Order, string> orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<Result<List<BookingResponse>>> Handle(GetAllBookingsQuery request, CancellationToken ct)
		{
			var orders = await _orderRepository.ListAsync(
				new OrdersByProviderIdSpec(request.ProviderId, request.Status), ct);

			var response = orders.Select(o => new BookingResponse(
				o.Id, o.CustomerJobId, o.ProviderId, o.FinalPrice.Amount, o.FinalPrice.Currency,
				o.StartDate, o.EndDate, o.Status.ToString())).ToList();

			return Result<List<BookingResponse>>.Success(response);
		}
	}
}
