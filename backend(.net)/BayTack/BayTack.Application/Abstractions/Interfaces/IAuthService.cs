using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Abstractions.Interfaces
{
	public interface IAuthService
	{
		Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto, CancellationToken ct = default);
		Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken ct = default);
		Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto, CancellationToken ct = default);
		Task<Result> RevokeTokenAsync(RevokeTokenRequestDto dto, CancellationToken ct = default);
		Task<Result> RevokeAllTokensForUserAsync(string userId, string? ipAddress, CancellationToken ct = default);
		Task<Result> ConfirmEmailAsync(ConfirmEmailDto dto, CancellationToken ct = default);
		Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken ct = default);
		Task<Result> ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken ct = default);

		Task<string?> GeneratePasswordResetTokenAsync(string email, CancellationToken ct = default);
		Task<Result> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken ct = default);

	}
}