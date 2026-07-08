using BayTack.Application.Abstractions.Messaging;
using FluentValidation;
using System;

namespace BayTack.Application.Features.Providers.Commands.SetAvailability
{
	public sealed class SetAvailabilityCommandValidator : AbstractValidator<SetAvailabilityCommand>
	{
		public SetAvailabilityCommandValidator()
		{
			RuleFor(x => x.ProviderProfileId).NotEmpty();
			RuleFor(x => x.DayOfWeek).IsInEnum();
			RuleFor(x => x)
				.Must(x => x.StartTime < x.EndTime)
				.WithMessage("Start time must be before end time.");
		}
	}

   
}
