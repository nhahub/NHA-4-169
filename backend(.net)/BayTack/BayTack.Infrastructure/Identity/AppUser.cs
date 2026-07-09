using BayTack.Domain.Enums;
using BayTack.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace BayTack.Infrastructure.Identity
{
	public class AppUser : IdentityUser<string>
	{
		public string FullName { get; private set; } = string.Empty;
		public UserStatus Status { get; private set; } = UserStatus.Active;
		public Address? Address { get; private set; }

		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; private set; }
		public string? UpdatedBy { get; private set; }

		public bool IsDeleted { get; private set; }
		public DateTime? DeletedAt { get; private set; }
		public string? DeletedBy { get; private set; }
		public string? DeleteReason { get; private set; }

		private AppUser() { }

		public static AppUser Create(string userName, string email, string fullName)
		{
			if (string.IsNullOrWhiteSpace(fullName))
				throw new ArgumentException("Full name is required.", nameof(fullName));

			return new AppUser
			{
				Id = Guid.NewGuid().ToString(),
				UserName = userName,
				Email = email,
				FullName = fullName,
				Status = UserStatus.Active
			};
		}

		public void UpdateProfile(string fullName, Address? address, string updatedBy)
		{
			if (string.IsNullOrWhiteSpace(fullName))
				throw new ArgumentException("Full name is required.", nameof(fullName));
			FullName = fullName;
			Address = address;
			UpdatedAt = DateTime.UtcNow;
			UpdatedBy = updatedBy;
		}

		public void Suspend() => Status = UserStatus.Suspended;
		public void Reactivate() => Status = UserStatus.Active;

		public void SoftDelete(string? deletedBy, string? reason)
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
			DeletedBy = deletedBy;
			DeleteReason = reason;
		}
	}
}