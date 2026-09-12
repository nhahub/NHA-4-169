using BayTack.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Services.Commands.CreateService
{
	public sealed record CreateServiceCommand(
		string CategoryId,
		string Name,
		string? Description,
		decimal MinPrice,
		decimal MaxPrice,
		string Currency = "EGP",
		bool AllowCredit = false,
		bool AllowInstallments = false,
		List<string>? PaymentMethodIds = null) : ICommand<string>;
}
