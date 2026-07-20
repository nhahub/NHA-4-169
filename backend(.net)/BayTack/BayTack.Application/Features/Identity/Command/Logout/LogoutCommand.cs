using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Identity.Command.Logout
{
    /// <summary>Revokes the refresh token server-side. Was missing entirely before - the
    /// frontend's "Logout" button could only ever clear its local session marker, leaving
    /// the httpOnly auth cookie/refresh token still valid until natural expiry.</summary>
    public sealed record LogoutCommand(string RefreshToken, string? IpAddress) : ICommand;
}
