using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Common.Models;
using BayTack.Application.Common.Security;
using BayTack.Domain.Enums;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace BayTack.Infrastructure.Services.Authentication
{

	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole<string>> _roles;
		private readonly ITokenService _tokenService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<RefreshToken, string> _refreshTokenRepository;
		//	//private readonly IEmailService _emailService;
		//	private readonly ICurrentUserService _currentUser;
		//	private readonly IDateTimeProvider _dateTime;
		//	private readonly ILogger<AuthService> _logger;
		public AuthService(
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			RoleManager<IdentityRole<string>> roles,
			ITokenService tokenService,
			IRepository<RefreshToken, string> refreshTokenRepository,
			IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roles = roles;
			_tokenService = tokenService;
			_refreshTokenRepository = refreshTokenRepository;
			_unitOfWork = unitOfWork;
		}

		
		public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
		{
			// Front-end sends "customer" | "provider" (lowercase) - map to the actual
			// seeded role names. Anything else is rejected; admins assign other roles
			// (e.g. Admin) exclusively through POST /users, never through public registration.
			var normalizedRole = dto.Role.Trim().ToLowerInvariant() switch
			{
				"customer" => "Customer",
				"provider" => "Provider",
				_ => null
			};

			if (normalizedRole is null)
				return Result<AuthResponseDto>.Failure("Invalid role specified.");

			var existing = await _userManager.FindByEmailAsync(dto.Email);
			if (existing is not null)
				return Result<AuthResponseDto>.Failure("Email already exists.");

			var user = AppUser.Create(userName: dto.Email, email: dto.Email, fullName: dto.FullName);
			if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
				user.PhoneNumber = dto.PhoneNumber;

			var createResult = await _userManager.CreateAsync(user, dto.Password);
			if (!createResult.Succeeded)
				return Result<AuthResponseDto>.Failure("Registration failed.");

			await _userManager.AddToRoleAsync(user, normalizedRole);

			return await IssueTokensAsync(user, dto.IpAddress, ct);
		}

		public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken ct = default)
		{
			var identifier = dto.Identifier?.Trim() ?? string.Empty;
			var user = await _userManager.FindByEmailAsync(identifier);
			if (user is null && identifier.Contains('@') is false)
			{
				// Not found by email and doesn't look like one - try matching by phone number.
				// UserManager has no FindByPhoneNumberAsync, so query directly.
				user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == identifier, ct);
			}
			if (user is null)
				return Result<AuthResponseDto>.Failure("Invalid email/phone or password.");

			if (user.Status == UserStatus.Suspended)
				return Result<AuthResponseDto>.Failure("User account is suspended.");
			var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
			if (signInResult.IsLockedOut)
				return Result<AuthResponseDto>.Failure("User account is locked out.");
			if (!signInResult.Succeeded)
				return Result<AuthResponseDto>.Failure("Invalid email/phone or password.");

			return await IssueTokensAsync(user, dto.IpAddress, ct);
		}
		 

		public async Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto, CancellationToken ct = default)
		{
			var existingToken = await FindTrackedTokenAsync(dto.RefreshToken, ct);
			if (existingToken is null)
				return Result<AuthResponseDto>.Failure("Invalid refresh token.");

			if (!existingToken.IsActive)
			{
				// Reusing a revoked/expired token is a signal the token may have leaked -
				// revoke every other active token for this user as a precaution.
				if (existingToken.IsRevoked)
					await RevokeAllTokensForUserAsync(existingToken.UserId, dto.IpAddress, ct);

				return Result<AuthResponseDto>.Failure("Invalid refresh token.");
			}

			var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
			if (user is null || user.Status == UserStatus.Suspended)
				return Result<AuthResponseDto>.Failure("Invalid refresh token.");

			var newTokenValue = _tokenService.GenerateRefreshTokenString();
			existingToken.Revoke(dto.IpAddress, replacedByToken: newTokenValue);
			_refreshTokenRepository.Update(existingToken);

			var result = await IssueTokensAsync(user, dto.IpAddress, ct, precomputedRefreshTokenValue: newTokenValue);
			return result;
		}

		public async Task<Result> RevokeTokenAsync(RevokeTokenRequestDto dto, CancellationToken ct = default)
		{
			var existingToken = await FindTrackedTokenAsync(dto.RefreshToken, ct);
			if (existingToken is null || !existingToken.IsActive)
				return Result.Failure("Invalid refresh token.");

			existingToken.Revoke(dto.IpAddress);
			_refreshTokenRepository.Update(existingToken);
			await _unitOfWork.SaveChangesAsync(ct);

			return Result.Success();
		}

		

		public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken ct = default)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user is null)
				return Result.Failure("User not found");

			var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
			return result.Succeeded ? Result.Success() : Result.Failure("Failed to change password");
		}
		
		public async Task<string?> GeneratePasswordResetTokenAsync(string email, CancellationToken ct = default)
		{
			var user = await _userManager.FindByEmailAsync(email);
			return user is null ? null : await _userManager.GeneratePasswordResetTokenAsync(user);
		}

		public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken ct = default)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user is null)
				return Result.Failure("Invalid reset token."); // same error either way - don't leak existence

			var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
			return result.Succeeded ? Result.Success() : Result.Failure("Invalid reset token.");
		}

		



		private async Task<Result<AuthResponseDto>> IssueTokensAsync(AppUser user, string? ipAddress, CancellationToken ct, string? precomputedRefreshTokenValue = null)
		{
			var roleNames = await _userManager.GetRolesAsync(user);
			var perms = new HashSet<string>();
			foreach (var rn in roleNames)
			{
				var role = await _roles.FindByNameAsync(rn);
				if (role == null) continue;
				var claims = await _roles.GetClaimsAsync(role);
				foreach (var c in claims.Where(c => c.Type == Permissions.ClaimType)) perms.Add(c.Value);
			}
			var FirstName = user.FullName?.Split(' ').FirstOrDefault() ?? string.Empty;
			var LastName = user.FullName?.Split(' ').Skip(1).FirstOrDefault() ?? string.Empty;

			var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email!, FirstName, LastName, roleNames, perms);
			var refreshToken = _tokenService.GenerateRefreshTokenString();

		    _refreshTokenRepository.Add(RefreshToken.Create(refreshToken, DateTime.UtcNow.AddDays(7), user.Id));

			await _unitOfWork.SaveChangesAsync(ct);
			var x = new AuthResponseDto(user.Id, user.Email ?? string.Empty,user.UserName ?? string.Empty,  user.FullName ?? string.Empty, accessToken.Token, accessToken.ExpiresAt, refreshToken, roleNames, perms);
			return Result<AuthResponseDto>.Success(x);
		}
 
		public async Task<Result> RevokeAllTokensForUserAsync(string userId, string? ipAddress, CancellationToken ct = default)
		{
			var activeTokens = await _refreshTokenRepository.ListAsync(new ActiveRefreshTokensForUserSpec(userId), ct);
			foreach (var token in activeTokens)
				token.Revoke(ipAddress);

			await _unitOfWork.SaveChangesAsync(ct);
			return Result.Success();
		}

		private Task<RefreshToken?> FindTrackedTokenAsync(string value, CancellationToken ct) =>
			_refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenByValueSpec(value), ct);



		public Task<Result> ConfirmEmailAsync(ConfirmEmailDto dto, CancellationToken ct = default)
		{
			throw new NotImplementedException();
		}

		public Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken ct = default)
		{
			throw new NotImplementedException();
		}

	}

}
