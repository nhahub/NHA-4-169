using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Identity.Command.ChangePassword
{
    public record ChangePasswordCommand(string UserId, string CurrentPassword, string NewPassword) : ICommand;
}
