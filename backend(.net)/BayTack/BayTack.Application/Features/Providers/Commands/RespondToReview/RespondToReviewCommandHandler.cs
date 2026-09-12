using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities;

namespace BayTack.Application.Features.Providers.Commands.RespondToReview
{
	public sealed class RespondToReviewCommandHandler : ICommandHandler<RespondToReviewCommand, RespondToReviewResponse>
	{
		private readonly IRepository<Review, string> _reviewRepository;

		public RespondToReviewCommandHandler(IRepository<Review, string> reviewRepository)
		{
			_reviewRepository = reviewRepository;
		}

		public async Task<Result<RespondToReviewResponse>> Handle(RespondToReviewCommand request, CancellationToken ct)
		{
			var review = await _reviewRepository.GetByIdAsync(request.ReviewId, ct);

			if (review is null)
				return Result<RespondToReviewResponse>.Failure("Review not found.");

			try
			{
				review.Respond(request.ResponseText);
			}
			catch (ArgumentException ex)
			{
				return Result<RespondToReviewResponse>.Failure(ex.Message);
			}

			_reviewRepository.Update(review);

			return Result<RespondToReviewResponse>.Success(new RespondToReviewResponse(review.Id, review.ProviderResponse!));
		}
	}
}
