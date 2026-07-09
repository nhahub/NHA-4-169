using FluentValidation;

namespace BayTack.Application.Features.Jobs.Commands.CreateRequest
{
    public sealed class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
    {
        public CreateRequestCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.ServiceId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.LocationDetails).NotEmpty();
            RuleFor(x => x.CityId).GreaterThan(0);
            RuleFor(x => x.Budget).GreaterThan(0).When(x => x.Budget.HasValue);
        }
    }
}