using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Identity.Command.RefreshToken
{
	public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthResponseDto>
	{
		private readonly IAuthService _authService;

		public RefreshTokenCommandHandler(IAuthService authService) => _authService = authService;

		public Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
		{
			return _authService.RefreshTokenAsync(new RefreshTokenRequestDto(request.AccessToken, request.RefreshToken, request.IpAddress));
		}
	}
}
