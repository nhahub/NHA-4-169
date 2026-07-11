using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.DTO.Auth;

namespace BayTack.Application.Features.Identity.Command.Login
{
    public record LoginCommand(string Email, string Password, string IpAddress) : ICommand<AuthResponseDto>;
}
