using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Identity.Command.Logout
{
    public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
    {
        private readonly IAuthService _authService;

        public LogoutCommandHandler(IAuthService authService) => _authService = authService;

        public Task<Result> Handle(LogoutCommand request, CancellationToken ct) =>
            _authService.RevokeTokenAsync(new RevokeTokenRequestDto(request.RefreshToken, request.IpAddress), ct);
    }
}
