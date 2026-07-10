using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.OrderAggregate;

namespace BayTack.Application.Features.Orders.Commands.UpdateBookingStatus
{
	public sealed class UpdateBookingStatusCommandHandler
		: ICommandHandler<UpdateBookingStatusCommand, UpdateBookingStatusResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateBookingStatusCommandHandler(IRepository<Order, string> orderRepository, IUnitOfWork unitOfWork)
		{
			_orderRepository = orderRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<UpdateBookingStatusResponse>> Handle(
			UpdateBookingStatusCommand request, CancellationToken ct)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);

			if (order is null)
				return Result<UpdateBookingStatusResponse>.Failure("Booking not found.");

			try
			{
				order.ChangeStatus(request.NewStatus, request.ChangedBy);
			}
			catch (InvalidOperationException ex)
			{
				return Result<UpdateBookingStatusResponse>.Failure(ex.Message);
			}

			_orderRepository.Update(order);
			await _unitOfWork.SaveChangesAsync(ct);

			return Result<UpdateBookingStatusResponse>.Success(
				new UpdateBookingStatusResponse(order.Id, order.Status.ToString()));
		}
	}
}
