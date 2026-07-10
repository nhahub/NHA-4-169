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
		private readonly IUnitOfWork _unitOfWork;

		public DeclineBookingCommandHandler(IRepository<Order, string> orderRepository, IUnitOfWork unitOfWork)
		{
			_orderRepository = orderRepository;
			_unitOfWork = unitOfWork;
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
			await _unitOfWork.SaveChangesAsync(ct);

			return Result<DeclineBookingResponse>.Success(new DeclineBookingResponse(order.Id, order.Status.ToString()));
		}
	}
}
