using FluentValidation;

namespace BayTack.Application.Features.Jobs.Commands.RetractBid
{
	public sealed class RetractBidCommandValidator : AbstractValidator<RetractBidCommand>
	{
		public RetractBidCommandValidator()
		{
			RuleFor(x => x.BidId).NotEmpty();
		}
	}
}
