using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Users.Command.DeleteUser
{

	public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
	{
		private readonly IIdentityService _identityService;

		public DeleteUserCommandHandler(IIdentityService identityService) => _identityService = identityService;

		public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken ct)
		{
			var result = await _identityService.SoftDeleteUserAsync(request.Id, request.DeletedBy, request.Reason, ct);
			return Result<bool>.Success(result.Value);
		}
	}
}
