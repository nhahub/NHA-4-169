using BayTack.Application.Abstractions.Messaging;


namespace BayTack.Application.Features.Users.Command.DeleteUser
{
	public sealed record DeleteUserCommand(string Id, string? DeletedBy, string? Reason) : ICommand<bool>;
}
