using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BayTack.Infrastructure.Identity
{
	public class RefreshToken : BaseEntity<string>
	{
		
		public string Value { get; set; }
		public DateTime ExpiryDate { get; set; }
		public bool IsRevoked { get; set; }
		public DateTime RevokedAt { get; private set; }
		public bool IsUsed { get; set; }
		public DateTime CreatedAt { get; set; }
		

		public string UserId { get; set; }
		public AppUser AppUser { get; set; } = null!;

		public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
		public bool IsActive => !IsRevoked && !IsUsed && !IsExpired; // at least one of these

		public string? RevokedByIp { get; private set; }
		public string? ReplacedByToken { get; private set; }

		public RefreshToken(string value, DateTime expiryDate, string userId)
		{
			Id = Guid.NewGuid().ToString();
			Value = value;
			ExpiryDate = expiryDate;
			UserId = userId;
			IsRevoked = false;
			IsUsed = false;
			CreatedAt = DateTime.UtcNow;
		}

		public static RefreshToken Create(string value, DateTime expiryDate, string userId)
		{
			return new RefreshToken(value, expiryDate, userId);
		}

		public void Revoke(string IPAddress, string? replacedByToken = null)
		{
			IsRevoked = true;
			RevokedAt = DateTime.UtcNow;
			RevokedByIp = IPAddress;
			ReplacedByToken = replacedByToken;
		}
	}
}
