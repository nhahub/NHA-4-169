using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Services.Commands.CreateService
{
	public sealed class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
	{
		public CreateServiceCommandValidator()
		{
			RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required.");
			RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Service name is required.");
			RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0).WithMessage("Min price must be non-negative.");
			RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(x => x.MinPrice).WithMessage("Max price must be greater than or equal to min price.");
		}
	}
}
