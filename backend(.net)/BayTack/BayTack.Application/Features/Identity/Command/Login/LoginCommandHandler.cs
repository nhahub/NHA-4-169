
using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Identity.Command.Register;
using MediatR;

namespace BayTack.Application.Features.Identity.Command.Login
{
	public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponseDto>
	{
		private readonly IAuthService _authService;

		public LoginCommandHandler(IAuthService authService) => _authService = authService;


		public Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			return _authService.LoginAsync(new LoginDto ( request.Email, request.Password, request.IpAddress));
		}
		
	}
}
