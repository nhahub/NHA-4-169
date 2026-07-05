using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Enums;
using BayTack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace BayTack.Domain.Entities.JobAggregate
{
	public class CustomerJob : SoftDeletableEntity<string>
	{
		public string CustomerId { get; private set; }
		public string ServiceId { get; private set; }
		public string Title { get; private set; } = string.Empty;
		public string Description { get; private set; } = string.Empty;
		public Address Location { get; private set; } = null!;
		public string? PreferredPayment { get; private set; }
		public JobStatus Status { get; private set; } = JobStatus.Open;

		private readonly List<JobImage> _images = new();
		public IReadOnlyCollection<JobImage> Images => _images.AsReadOnly();

		private readonly List<ProviderBid> _bids = new();
		public IReadOnlyCollection<ProviderBid> Bids => _bids.AsReadOnly();

		private CustomerJob() { }

		public static CustomerJob Create(string customerId, string serviceId, string title, string description,
			Address location, string? preferredPayment = null)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Title is required.", nameof(title));

			return new CustomerJob
			{
				CustomerId = customerId,
				ServiceId = serviceId,
				Title = title,
				Description = description,
				Location = location,
				PreferredPayment = preferredPayment,
				Status = JobStatus.Open
			};
		}

		public void AddImage(string imageUrl) => _images.Add(JobImage.Create(Id, imageUrl));

		public ProviderBid PlaceBid(string providerId, Money proposedPrice, int durationInDays, string? notes)
		{
			if (Status != JobStatus.Open && Status != JobStatus.InBidding)
				throw new InvalidOperationException("Bids can only be placed on open jobs.");
			if (_bids.Any(b => b.ProviderId == providerId && b.Status == BidStatus.Pending))
				throw new InvalidOperationException("Provider already has a pending bid on this job.");

			var bid = ProviderBid.Create(Id, providerId, proposedPrice, durationInDays, notes);
			_bids.Add(bid);
			Status = JobStatus.InBidding;
			return bid;
		}

		public void AcceptBid(string bidId)
		{
			var bid = _bids.FirstOrDefault(b => b.Id == bidId)
				?? throw new InvalidOperationException("Bid not found.");

			bid.Accept();
			foreach (var other in _bids.Where(b => b.Id != bidId && b.Status == BidStatus.Pending))
				other.Reject();

			Status = JobStatus.Assigned;
		}

		public void Cancel(string cancelledBy, string reason)
		{
			if (Status == JobStatus.Completed)
				throw new InvalidOperationException("Cannot cancel a completed job.");
			Status = JobStatus.Cancelled;
			Delete(cancelledBy, reason);
		}
	}

}
