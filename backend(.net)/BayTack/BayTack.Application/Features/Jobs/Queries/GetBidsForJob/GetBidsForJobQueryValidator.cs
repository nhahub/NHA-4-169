using FluentValidation;

namespace BayTack.Application.Features.Jobs.Queries.GetBidsForJob
{
	public sealed class GetBidsForJobQueryValidator : AbstractValidator<GetBidsForJobQuery>
	{
		public GetBidsForJobQueryValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
		}
	}
}
