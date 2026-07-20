using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.DTO.Auth;

namespace BayTack.Application.Features.Identity.Command.RefreshToken
{
    public record RefreshTokenCommand(string AccessToken, string RefreshToken, string? IpAddress) : ICommand<AuthResponseDto>;
}
