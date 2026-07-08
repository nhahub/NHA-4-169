using FluentValidation;

namespace BayTack.Application.Features.Orders.Queries.GetMyOrders
{
	public sealed class GetMyOrdersQueryValidator : AbstractValidator<GetMyOrdersQuery>
	{
		private static readonly string[] AllowedStatuses = { "active", "completed", "cancelled" };

		public GetMyOrdersQueryValidator()
		{
			RuleFor(x => x.CustomerId).NotEmpty();

			RuleFor(x => x.Status)
				.Must(s => string.IsNullOrWhiteSpace(s) || AllowedStatuses.Contains(s.ToLowerInvariant()))
				.WithMessage("Status must be one of: active, completed, cancelled.");
		}
	}
}
