using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Users.Queries.GetAllUsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Command
{
	public sealed record CreateUserCommand(string Name, string Email, string? Phone, string Role) : ICommand<UserResponse>;

}
