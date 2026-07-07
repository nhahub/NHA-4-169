using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Users.Queries.GetAllUsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Queries.GetUserById
{
	public sealed record GetUserByIdQuery(string Id) : IQuery<UserResponse>;

}
