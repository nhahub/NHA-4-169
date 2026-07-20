using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Orders.Common;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Orders.Commands.CancelOrder
{
	public sealed class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, OrderResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;
		private readonly IRepository<CustomerJob, string> _jobRepository;
		private readonly IOrdersReadRepository _ordersReadRepository;

		public CancelOrderCommandHandler(
			IRepository<Order, string> orderRepository,
			IRepository<CustomerJob, string> jobRepository,
			IOrdersReadRepository ordersReadRepository)
		{
			_orderRepository = orderRepository;
			_jobRepository = jobRepository;
			_ordersReadRepository = ordersReadRepository;
		}

		public async Task<Result<OrderResponse>> Handle(CancelOrderCommand request, CancellationToken ct)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);
			if (order is null)
				return Result<OrderResponse>.Failure("Order not found.");

			var job = await _jobRepository.GetByIdAsync(order.CustomerJobId, ct);
			// Ownership check: don't reveal to one customer that another customer's order id exists.
			if (job is null || job.CustomerId != request.CustomerId)
				return Result<OrderResponse>.Failure("Order not found.");

			try
			{
				order.ChangeStatus(OrderStatus.Cancelled, request.CustomerId);
			}
			catch (InvalidOperationException ex)
			{
				return Result<OrderResponse>.Failure(ex.Message);
			}

			_orderRepository.Update(order);
			// NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

			var providerName = await _ordersReadRepository.GetProviderNameAsync(order.ProviderId, ct) ?? "Unknown provider";
			var status = order.Status.ToString();

			return new OrderResponse(
				order.Id,
				job.ServiceId,
				job.Title,
				providerName,
				null,
				order.FinalPrice.Amount,
				status,
				OrderResponse.ProgressFor(status),
				order.CreatedAt
				);
		}
	}
}
