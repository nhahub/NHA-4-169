using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Common.Models;
using BayTack.Infrastructure.Persistence;
using BayTack.Infrastructure.Persistence.Configurations;
using Microsoft.Extensions.Logging;

namespace BayTack.Infrastructure.Services.Authentication
{

	//public class AuthService : IAuthService
	//{
	//	private readonly IIdentityService _identityService;
	//	private readonly ITokenService _tokenService;
	//	//private readonly IEmailService _emailService;
	//	private readonly AppDbContext _context;
	//	private readonly ICurrentUserService _currentUser;
	//	private readonly IDateTimeProvider _dateTime;
	//	private readonly ILogger<AuthService> _logger;

	//	private const int RefreshTokenDaysToLive = 7;

	//	public AuthService(
	//		IIdentityService identityService,
	//		ITokenService tokenService,
	//		//IEmailService emailService,
	//		AppDbContext context,
	//		ICurrentUserService currentUser,
	//		IDateTimeProvider dateTime,
	//		ILogger<AuthService> logger)
	//	{
	//		_identityService = identityService;
	//		_tokenService = tokenService;
	//		//_emailService = emailService;
	//		_context = context;
	//		_currentUser = currentUser;
	//		_dateTime = dateTime;
	//		_logger = logger;
	//	}

	//	public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
	//	{
	//		if (dto.Password != dto.ConfirmPassword)
	//			return Result<AuthResponseDto>.Failure("Password and confirmation password do not match.");

	//		var createResult = await _identityService.CreateUserAsync(dto.FirstName, dto.LastName, dto.Email, dto.UserName, dto.Password, ct);
	//		if (!createResult.IsSuccess || createResult.Value is null)
	//			return Result<AuthResponseDto>.Failure(createResult.Error!);

	//		var user = createResult.Value!;

	//		// Fire off the email confirmation link (does not block registration success on delivery failure)
	//		try
	//		{
	//			var confirmToken = await _identityService.GenerateEmailConfirmationTokenAsync(user.UserId);
	//			await _emailService.SendEmailConfirmationAsync(user.Email, user.UserId.ToString(), confirmToken, ct);
	//		}
	//		catch (Exception ex)
	//		{
	//			_logger.LogWarning(ex, "Failed to send confirmation email to {Email}", user.Email);
	//		}

	//		await _identityService.AssignRolesAsync(user.UserId, new[] { Domain.Constants.Roles.User });

	//		var authResponse = await BuildAuthResponseAsync(user.UserId, user.Email, user.UserName, $"{user.FirstName} {user.LastName}".Trim(), ct);
	//		return Result<AuthResponseDto>.Success(authResponse);
	//	}

	//	public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken ct = default)
	//	{
	//		var user = await _identityService.FindByEmailOrUserNameAsync(dto.EmailOrUserName);
	//		if (user is null)
	//			return Result<AuthResponseDto>.Failure("Invalid credentials.");

	//		if (await _identityService.IsLockedOutAsync(user.UserId))
	//			return Result<AuthResponseDto>.Failure("Account is temporarily locked due to multiple failed attempts. Try again later.");

	//		var passwordValid = await _identityService.CheckPasswordAsync(user.UserId, dto.Password);
	//		if (!passwordValid)
	//		{
	//			await _identityService.RegisterFailedAttemptAsync(user.UserId);
	//			return Result<AuthResponseDto>.Failure("Invalid credentials.");
	//		}

	//		if (!await _identityService.IsEmailConfirmedAsync(user.UserId))
	//			return Result<AuthResponseDto>.Failure("Email is not confirmed. Please check your inbox.");

	//		await _identityService.ResetAccessFailedCountAsync(user.UserId);
	//		await _identityService.UpdateLastLoginAsync(user.UserId);

	//		var authResponse = await BuildAuthResponseAsync(user.UserId, user.Email, user.UserName, $"{user.FirstName} {user.LastName}".Trim(), ct);
	//		return Result<AuthResponseDto>.Success(authResponse);
	//	}

	//	public async Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto, CancellationToken ct = default)
	//	{
	//		var principal = _tokenService.GetPrincipalFromExpiredToken(dto.AccessToken);
	//		if (principal is null)
	//			return Result<AuthResponseDto>.Failure("Invalid access token.");

	//		var jwtId = principal.FindFirst("jti")?.Value;
	//		var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
	//		if (jwtId is null || userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
	//			return Result<AuthResponseDto>.Failure("Invalid token payload.");

	//		var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == dto.RefreshToken, ct);
	//		if (storedToken is null)
	//			return Result<AuthResponseDto>.Failure("Refresh token does not exist.");

	//		if (storedToken.JwtId != jwtId)
	//			return Result<AuthResponseDto>.Failure("Refresh token does not match this access token.");

	//		if (!storedToken.IsActive)
	//			return Result<AuthResponseDto>.Failure("Refresh token is no longer active.");

	//		var user = await _identityService.FindByIdAsync(storedToken.UserId);
	//		if (user is null)
	//			return Result<AuthResponseDto>.Failure("User no longer exists.");

	//		// rotate: invalidate old token, issue a new pair
	//		storedToken.Used = true;
	//		var authResponse = await BuildAuthResponseAsync(user.UserId, user.Email, user.UserName, $"{user.FirstName} {user.LastName}".Trim(), ct);
	//		storedToken.ReplacedByToken = authResponse.RefreshToken;
	//		await _context.SaveChangesAsync(ct);

	//		return Result<AuthResponseDto>.Success(authResponse);
	//	}

	//	public async Task<Result> RevokeTokenAsync(RevokeTokenRequestDto dto, CancellationToken ct = default)
	//	{
	//		var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == dto.RefreshToken, ct);
	//		if (storedToken is null)
	//			return Result.Failure("Refresh token not found.");

	//		storedToken.Invalidated = true;
	//		storedToken.RevokedAt = _dateTime.UtcNow;
	//		storedToken.RevokedByIp = _currentUser.IpAddress;
	//		await _context.SaveChangesAsync(ct);

	//		return Result.Success();
	//	}

	//	public async Task<Result> ConfirmEmailAsync(ConfirmEmailDto dto, CancellationToken ct = default)
	//		=> await _identityService.ConfirmEmailAsync(dto.UserId, dto.Token);

	//	public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken ct = default)
	//	{
	//		var user = await _identityService.FindByEmailOrUserNameAsync(dto.Email);
	//		// Always return success to avoid user enumeration, but only actually send if the user exists.
	//		if (user is not null)
	//		{
	//			var token = await _identityService.GeneratePasswordResetTokenAsync(dto.Email);
	//			await _emailService.SendPasswordResetAsync(user.Email, user.Email, token, ct);
	//		}

	//		return Result.Success();
	//	}

	//	public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken ct = default)
	//	{
	//		if (dto.NewPassword != dto.ConfirmPassword)
	//			return Result.Failure("Password and confirmation password do not match.");

	//		return await _identityService.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);
	//	}

	//	public async Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken ct = default)
	//	{
	//		if (dto.NewPassword != dto.ConfirmPassword)
	//			return Result.Failure("Password and confirmation password do not match.");

	//		return await _identityService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
	//	}

	//	private async Task<AuthResponseDto> BuildAuthResponseAsync(Guid userId, string email, string userName, string fullName, CancellationToken ct)
	//	{
	//		var roles = await _identityService.GetRolesAsync(userId);
	//		var permissions = await _identityService.GetPermissionsAsync(userId);

	//		var applicationUser = new ApplicationUser { Id = userId, Email = email, UserName = userName };
	//		var accessToken = _tokenService.GenerateAccessToken(applicationUser, roles, permissions);
	//		var refreshTokenString = _tokenService.GenerateRefreshTokenString();

	//		_context.RefreshTokens.Add(new RefreshToken
	//		{
	//			Token = refreshTokenString,
	//			JwtId = accessToken.JwtId,
	//			UserId = userId,
	//			ExpiryDate = _dateTime.UtcNow.AddDays(RefreshTokenDaysToLive),
	//			CreatedByIp = _currentUser.IpAddress
	//		});
	//		await _context.SaveChangesAsync(ct);

	//		return new AuthResponseDto(
	//			userId, email, userName, fullName,
	//			accessToken.Token, accessToken.ExpiresAt,
	//			refreshTokenString, roles);
	//	}
	//}
}
