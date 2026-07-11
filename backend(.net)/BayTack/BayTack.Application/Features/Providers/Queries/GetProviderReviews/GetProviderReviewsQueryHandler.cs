using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Orders.Queries.GetAllBookings;
using BayTack.Domain.Entities;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviews
{
	public sealed class GetProviderReviewsQueryHandler : IQueryHandler<GetProviderReviewsQuery, ProviderReviewsListResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;
		private readonly IRepository<Review, string> _reviewRepository;
		private readonly IRepository<CustomerJob, string> _jobRepository;
		private readonly IRepository<Service, string> _serviceRepository;
		private readonly IUserRepository _userRepository;

		public GetProviderReviewsQueryHandler(
			IRepository<Order, string> orderRepository,
			IRepository<Review, string> reviewRepository,
			IRepository<CustomerJob, string> jobRepository,
			IRepository<Service, string> serviceRepository,
			IUserRepository userRepository)
		{
			_orderRepository = orderRepository;
			_reviewRepository = reviewRepository;
			_jobRepository = jobRepository;
			_serviceRepository = serviceRepository;
			_userRepository = userRepository;
		}

		public async Task<Result<ProviderReviewsListResponse>> Handle(GetProviderReviewsQuery request, CancellationToken ct)
		{
			var orders = await _orderRepository.ListAsync(new OrdersByProviderIdSpec(request.ProviderId, null), ct);
			var orderIds = orders.Select(o => o.Id).ToList();

			if (orderIds.Count == 0)
				return Result<ProviderReviewsListResponse>.Success(new ProviderReviewsListResponse(new List<ReviewResponse>(), 0));

			var allReviews = await _reviewRepository.ListAsync(new ReviewsByOrderIdsSpec(orderIds), ct);

			if (int.TryParse(request.Filter, out var ratingFilter) && ratingFilter is >= 1 and <= 5)
				allReviews = allReviews.Where(r => r.Rating == ratingFilter).ToList();

			var total = allReviews.Count;
			var paged = allReviews.OrderByDescending(r => r.CreatedAt)
				.Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToList();

			var jobIdByOrderId = orders.ToDictionary(o => o.Id, o => o.CustomerJobId);
			var jobIds = paged.Select(r => jobIdByOrderId.GetValueOrDefault(r.OrderId))
				.Where(id => id != null).Distinct().Cast<string>().ToList();
			var jobs = jobIds.Count > 0 ? await _jobRepository.ListAsync(new CustomerJobsByIdsSpec(jobIds), ct) : new List<CustomerJob>();

			var serviceIds = jobs.Select(j => j.ServiceId).Distinct().ToList();
			var services = serviceIds.Count > 0 ? await _serviceRepository.ListAsync(new ServicesByIdsSpec(serviceIds), ct) : new List<Service>();

			var serviceNameByJobId = jobs.ToDictionary(j => j.Id, j => services.FirstOrDefault(s => s.Id == j.ServiceId)?.Name);

			var responses = new List<ReviewResponse>();
			foreach (var review in paged)
			{
				var author = await _userRepository.GetByIdAsync(review.CustomerId, ct);
				var jobId = jobIdByOrderId.GetValueOrDefault(review.OrderId);
				var serviceName = jobId != null ? serviceNameByJobId.GetValueOrDefault(jobId) : null;

				responses.Add(new ReviewResponse(
					review.Id, review.CustomerId, author?.FullName ?? "Unknown",
					review.Rating, review.ReviewText, serviceName, review.OrderId, review.CreatedAt, review.ProviderResponse));
			}

			return Result<ProviderReviewsListResponse>.Success(new ProviderReviewsListResponse(responses, total));
		}
	}
}
