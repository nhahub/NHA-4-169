using FluentValidation;

namespace BayTack.Application.Features.Jobs.Commands.DeleteRequest
{
    public sealed class DeleteRequestCommandValidator : AbstractValidator<DeleteRequestCommand>
    {
        public DeleteRequestCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
        }
    }
}