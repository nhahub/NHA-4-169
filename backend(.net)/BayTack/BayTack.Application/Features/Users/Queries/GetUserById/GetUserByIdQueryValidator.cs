using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Queries.GetUserById
{

	public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
	{
		public GetUserByIdQueryValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
		}
	}

}
