using FluentValidation;

namespace BayTack.Application.Features.Messages.Commands.SendMessage
{
	public sealed class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
	{
		public SendMessageCommandValidator()
		{
			RuleFor(x => x.CustomerId).NotEmpty();
			RuleFor(x => x.ConversationId).NotEmpty();
			RuleFor(x => x.Text).NotEmpty().MaximumLength(4000);
		}
	}
}
