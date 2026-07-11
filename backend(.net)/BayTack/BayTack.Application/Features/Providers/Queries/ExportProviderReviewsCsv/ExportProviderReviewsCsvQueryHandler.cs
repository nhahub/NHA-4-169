using System.Text;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Orders.Queries.GetAllBookings;
using BayTack.Application.Features.Providers.Queries.GetProviderReviews;
using BayTack.Domain.Entities;
using BayTack.Domain.Entities.OrderAggregate;

namespace BayTack.Application.Features.Providers.Queries.ExportProviderReviewsCsv
{
	public sealed class ExportProviderReviewsCsvQueryHandler : IQueryHandler<ExportProviderReviewsCsvQuery, string>
	{
		private readonly IRepository<Order, string> _orderRepository;
		private readonly IRepository<Review, string> _reviewRepository;

		public ExportProviderReviewsCsvQueryHandler(
			IRepository<Order, string> orderRepository, IRepository<Review, string> reviewRepository)
		{
			_orderRepository = orderRepository;
			_reviewRepository = reviewRepository;
		}

		public async Task<Result<string>> Handle(ExportProviderReviewsCsvQuery request, CancellationToken ct)
		{
			var orders = await _orderRepository.ListAsync(new OrdersByProviderIdSpec(request.ProviderId, null), ct);
			var orderIds = orders.Select(o => o.Id).ToList();

			var reviews = orderIds.Count > 0
				? await _reviewRepository.ListAsync(new ReviewsByOrderIdsSpec(orderIds), ct)
				: new List<Review>();

			if (int.TryParse(request.Filter, out var ratingFilter) && ratingFilter is >= 1 and <= 5)
				reviews = reviews.Where(r => r.Rating == ratingFilter).ToList();

			var sb = new StringBuilder();
			sb.AppendLine("ReviewId,OrderId,CustomerId,Rating,Text,ProviderResponse,CreatedAt");
			foreach (var r in reviews)
			{
				var text = (r.ReviewText ?? string.Empty).Replace("\"", "\"\"");
				var response = (r.ProviderResponse ?? string.Empty).Replace("\"", "\"\"");
				sb.AppendLine($"{r.Id},{r.OrderId},{r.CustomerId},{r.Rating},\"{text}\",\"{response}\",{r.CreatedAt:O}");
			}

			return Result<string>.Success(sb.ToString());
		}
	}
}
