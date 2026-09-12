using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Orders.Commands.DeclineBooking
{
	public sealed class DeclineBookingCommandHandler : ICommandHandler<DeclineBookingCommand, DeclineBookingResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;

		public DeclineBookingCommandHandler(IRepository<Order, string> orderRepository )
		{
			_orderRepository = orderRepository;
		}

		public async Task<Result<DeclineBookingResponse>> Handle(DeclineBookingCommand request, CancellationToken ct)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);

			if (order is null)
				return Result<DeclineBookingResponse>.Failure("Booking not found.");

			try
			{
				order.ChangeStatus(OrderStatus.Cancelled, request.ChangedBy);
			}
			catch (InvalidOperationException ex)
			{
				return Result<DeclineBookingResponse>.Failure(ex.Message);
			}

			_orderRepository.Update(order);

			return Result<DeclineBookingResponse>.Success(new DeclineBookingResponse(order.Id, order.Status.ToString()));
		}
	}
}
