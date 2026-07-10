using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Orders.Commands.CompleteBooking
{
	public sealed class CompleteBookingCommandHandler : ICommandHandler<CompleteBookingCommand, CompleteBookingResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CompleteBookingCommandHandler(IRepository<Order, string> orderRepository, IUnitOfWork unitOfWork)
		{
			_orderRepository = orderRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CompleteBookingResponse>> Handle(CompleteBookingCommand request, CancellationToken ct)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);

			if (order is null)
				return Result<CompleteBookingResponse>.Failure("Booking not found.");

			try
			{
				order.ChangeStatus(OrderStatus.Completed, request.ChangedBy);
			}
			catch (InvalidOperationException ex)
			{
				return Result<CompleteBookingResponse>.Failure(ex.Message);
			}

			_orderRepository.Update(order);
			await _unitOfWork.SaveChangesAsync(ct);

			return Result<CompleteBookingResponse>.Success(
				new CompleteBookingResponse(order.Id, order.Status.ToString(), order.EndDate));
		}
	}
}
