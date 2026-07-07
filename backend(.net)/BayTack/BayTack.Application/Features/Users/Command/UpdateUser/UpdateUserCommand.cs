using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Users.Queries.GetAllUsers;

namespace BayTack.Application.Features.Users.Command.UpdateUser
{
	public sealed record UpdateUserCommand(string Id, string Name, string Email, string? Phone, string? Role, string UpdatedBy)
	: ICommand<UserResponse>;

}
