using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Orders.Queries.GetAllBookings;
using BayTack.Domain.Entities.OrderAggregate;

namespace BayTack.Application.Features.Orders.Queries.GetBookingById
{
	public sealed class GetBookingByIdQueryHandler : IQueryHandler<GetBookingByIdQuery, BookingResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;

		public GetBookingByIdQueryHandler(IRepository<Order, string> orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task<Result<BookingResponse>> Handle(GetBookingByIdQuery request, CancellationToken ct)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);

			if (order is null)
				return Result<BookingResponse>.Failure("Booking not found.");

			var response = new BookingResponse(
				order.Id, order.CustomerJobId, order.ProviderId, order.FinalPrice.Amount, order.FinalPrice.Currency,
				order.StartDate, order.EndDate, order.Status.ToString());

			return Result<BookingResponse>.Success(response);
		}
	}
}
