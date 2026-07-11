using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Identity.Command.Register
{
	public sealed record RegisterCommand(string FullName, string Email, string Password, string? Phone, string Role, string? IpAddress)
	: ICommand<AuthResponseDto>;

}
