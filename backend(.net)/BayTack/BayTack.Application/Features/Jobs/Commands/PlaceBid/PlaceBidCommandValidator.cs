using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Jobs.Commands.PlaceBid
{

	public sealed class PlaceBidCommandValidator : AbstractValidator<PlaceBidCommand>
	{
		public PlaceBidCommandValidator()
		{
			RuleFor(x => x.JobId).GreaterThan(0);
			RuleFor(x => x.ProviderId).GreaterThan(0);
			RuleFor(x => x.ProposedPrice).GreaterThan(0);
			RuleFor(x => x.Currency).NotEmpty().Length(3);
			RuleFor(x => x.DurationInDays).GreaterThan(0).LessThanOrEqualTo(365);
			RuleFor(x => x.Notes).MaximumLength(2000);
		}
	}
}
