using FluentValidation;

namespace BayTack.Application.Features.Orders.Queries.GetBookingById
{
	public sealed class GetBookingByIdQueryValidator : AbstractValidator<GetBookingByIdQuery>
	{
		public GetBookingByIdQueryValidator()
		{
			RuleFor(x => x.OrderId).NotEmpty();
		}
	}
}
