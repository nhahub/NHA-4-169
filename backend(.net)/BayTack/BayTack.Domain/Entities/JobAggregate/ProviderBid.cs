using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Enums;
using BayTack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace BayTack.Domain.Entities.JobAggregate
{
	public class ProviderBid : AuditableEntity<string>
	{
		public string CustomerJobId { get; private set; }
		public string ProviderId { get; private set; }
		public Money ProposedPrice { get; private set; } = Money.Zero();
		public int DurationInDays { get; private set; }
		public string? Notes { get; private set; }
		public BidStatus Status { get; private set; } = BidStatus.Pending;

		private ProviderBid() { }

		internal static ProviderBid Create(string jobId, string providerId, Money proposedPrice, int durationInDays, string? notes)
		{
			if (durationInDays <= 0)
				throw new ArgumentException("Duration must be positive.", nameof(durationInDays));

			return new ProviderBid
			{
				CustomerJobId = jobId,
				ProviderId = providerId,
				ProposedPrice = proposedPrice,
				DurationInDays = durationInDays,
				Notes = notes
			};
		}

		public void Accept()
		{
			if (Status != BidStatus.Pending) throw new InvalidOperationException("Only pending bids can be accepted.");
			Status = BidStatus.Accepted;
		}

		public void Reject()
		{
			if (Status == BidStatus.Pending) Status = BidStatus.Rejected;
		}

		public void Withdraw()
		{
			if (Status != BidStatus.Pending) throw new InvalidOperationException("Only pending bids can be withdrawn.");
			Status = BidStatus.Withdrawn;
		}
	}
}
