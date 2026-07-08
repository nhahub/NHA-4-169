using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Users.Command.DeactivateUser
{
	public sealed class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand, bool>
	{
		private readonly IIdentityService _identityService;

		public DeactivateUserCommandHandler(IIdentityService identityService) => _identityService = identityService;

		public Task<Result<bool>> Handle(DeactivateUserCommand request, CancellationToken ct) =>
			_identityService.DeactivateUserAsync(request.Id, ct);
	}

}
