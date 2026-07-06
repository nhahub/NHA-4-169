using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Queries.GetAllUsers
{
	public sealed class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
	{
		public GetAllUsersQueryValidator()
		{
			RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
			RuleFor(x => x.Limit).InclusiveBetween(1, 100);
			RuleFor(x => x.Search).MaximumLength(200);
			RuleFor(x => x.Role).MaximumLength(50);
		}
	}
}
