using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Identity.Command.ChangePassword
{
	public sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
	{
		private readonly IAuthService _authService;

		public ChangePasswordCommandHandler(IAuthService authService) => _authService = authService;

		public Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
		{
			return _authService.ChangePasswordAsync(
				request.UserId,
				new ChangePasswordDto(request.CurrentPassword, request.NewPassword),
				cancellationToken);
		}
	}
}
