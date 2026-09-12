using BayTack.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Services.Commands.DeleteService
{
	public sealed record DeleteServiceCommand(string Id) : ICommand;
}
