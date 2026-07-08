using FluentValidation;

namespace BayTack.Application.Features.Orders.Queries.GetOrderById
{
	public sealed class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
	{
		public GetOrderByIdQueryValidator()
		{
			RuleFor(x => x.CustomerId).NotEmpty();
			RuleFor(x => x.OrderId).NotEmpty();
		}
	}
}
