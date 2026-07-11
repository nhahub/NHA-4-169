using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Identity.Command.Register
{

	public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResponseDto>
	{
		private readonly IAuthService _authService;

		public RegisterCommandHandler(IAuthService authService) => _authService = authService;

		public Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken ct) =>
			_authService.RegisterAsync(new RegisterDto(request.FullName, request.Email, request.Password, request.Phone, request.Role, request.IpAddress), ct);
	}

}


