using BayTack.Application.Abstractions.Messaging;


namespace BayTack.Application.Features.Users.Command.DeactivateUser
{
	public sealed record DeactivateUserCommand(string Id) : ICommand<bool>;

}
