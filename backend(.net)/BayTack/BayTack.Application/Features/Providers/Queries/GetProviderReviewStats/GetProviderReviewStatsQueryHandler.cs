using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Orders.Queries.GetAllBookings;
using BayTack.Application.Features.Providers.Queries.GetProviderReviews;
using BayTack.Domain.Entities;
using BayTack.Domain.Entities.OrderAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviewStats
{
	public sealed class GetProviderReviewStatsQueryHandler
		: IQueryHandler<GetProviderReviewStatsQuery, ProviderReviewStatsResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;
		private readonly IRepository<Review, string> _reviewRepository;

		public GetProviderReviewStatsQueryHandler(
			IRepository<Order, string> orderRepository, IRepository<Review, string> reviewRepository)
		{
			_orderRepository = orderRepository;
			_reviewRepository = reviewRepository;
		}

		public async Task<Result<ProviderReviewStatsResponse>> Handle(
			GetProviderReviewStatsQuery request, CancellationToken ct)
		{
			var orders = await _orderRepository.ListAsync(new OrdersByProviderIdSpec(request.ProviderId, null), ct);
			var orderIds = orders.Select(o => o.Id).ToList();

			var reviews = orderIds.Count > 0
				? await _reviewRepository.ListAsync(new ReviewsByOrderIdsSpec(orderIds), ct)
				: new List<Review>();

			var total = reviews.Count;
			var average = total > 0 ? reviews.Average(r => r.Rating) : 0;
			var satisfactionPct = total > 0 ? reviews.Count(r => r.Rating >= 4) * 100.0 / total : 0;
			var fiveStar = reviews.Count(r => r.Rating == 5);
			var weekly = reviews.Count(r => r.CreatedAt >= DateTime.UtcNow.AddDays(-7));

			var response = new ProviderReviewStatsResponse(average, total, satisfactionPct, fiveStar, weekly);
			return Result<ProviderReviewStatsResponse>.Success(response);
		}
	}
}
