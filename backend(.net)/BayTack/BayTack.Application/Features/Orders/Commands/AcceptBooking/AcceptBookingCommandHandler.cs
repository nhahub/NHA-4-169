using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Orders.Commands.AcceptBooking
{
	public sealed class AcceptBookingCommandHandler : ICommandHandler<AcceptBookingCommand, AcceptBookingResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;

		public AcceptBookingCommandHandler(IRepository<Order, string> orderRepository )
		{
			_orderRepository = orderRepository;
		}

		public async Task<Result<AcceptBookingResponse>> Handle(AcceptBookingCommand request, CancellationToken ct)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);

			if (order is null)
				return Result<AcceptBookingResponse>.Failure("Booking not found.");

			try
			{
				order.ChangeStatus(OrderStatus.Confirmed, request.ChangedBy);
			}
			catch (InvalidOperationException ex)
			{
				return Result<AcceptBookingResponse>.Failure(ex.Message);
			}

			_orderRepository.Update(order);

			return Result<AcceptBookingResponse>.Success(new AcceptBookingResponse(order.Id, order.Status.ToString()));
		}
	}
}
