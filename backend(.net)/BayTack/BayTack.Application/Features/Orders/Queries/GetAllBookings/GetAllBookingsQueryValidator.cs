using FluentValidation;

namespace BayTack.Application.Features.Orders.Queries.GetAllBookings
{
	public sealed class GetAllBookingsQueryValidator : AbstractValidator<GetAllBookingsQuery>
	{
		public GetAllBookingsQueryValidator()
		{
			RuleFor(x => x.ProviderId).NotEmpty();
		}
	}
}
