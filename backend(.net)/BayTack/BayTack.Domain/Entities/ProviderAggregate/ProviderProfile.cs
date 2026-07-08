using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Enums;
using BayTack.Domain.ValueObjects;
 

namespace BayTack.Domain.Entities.ProviderAggregate
{
	public class ProviderProfile : AuditableEntity<string>
	{
		public string UserId { get; private set; }
		public ProviderType ProviderType { get; private set; }
		public string? Bio { get; private set; }
		public int YearsOfExperience { get; private set; }
		public Address? WorkshopAddress { get; private set; }
		public VerificationStatus VerificationStatus { get; private set; } = VerificationStatus.Pending;

		private readonly List<ProviderDocument> _documents = new();
		public IReadOnlyCollection<ProviderDocument> Documents => _documents.AsReadOnly();

		private readonly List<ProviderPortfolioItem> _portfolio = new();
		public IReadOnlyCollection<ProviderPortfolioItem> Portfolio => _portfolio.AsReadOnly();

		private readonly List<ProviderAvailability> _availabilities = new();
		public IReadOnlyCollection<ProviderAvailability> Availabilities => _availabilities.AsReadOnly();

		private ProviderProfile() { }

		public static ProviderProfile Create(string userId, ProviderType type, int yearsOfExperience, string? bio = null)
		{
			if (yearsOfExperience < 0)
				throw new ArgumentException("Years of experience cannot be negative.", nameof(yearsOfExperience));

			return new ProviderProfile
			{
			    Id = Guid.NewGuid().ToString(),
				UserId = userId,
				ProviderType = type,
				YearsOfExperience = yearsOfExperience,
				Bio = bio
			};
		}

		public void UpdateBio(string bio, string updatedBy)
		{
			Bio = bio;
			SetUpdated(updatedBy);
		}

		public void SetWorkshopAddress(Address address, string updatedBy)
		{
			WorkshopAddress = address;
			SetUpdated(updatedBy);
		}

		public ProviderDocument AddDocument(string docType, string docUrl)
		{
			var doc = ProviderDocument.Create(Id, docType, docUrl);
			_documents.Add(doc);
			return doc;
		}

		public void Verify()
		{
			if (!_documents.Any() || _documents.Any(d => d.Status != DocumentStatus.Approved))
				throw new InvalidOperationException("All documents must be approved before a provider can be verified.");
			VerificationStatus = VerificationStatus.Approved;
		}

		public void Reject() => VerificationStatus = VerificationStatus.Rejected;

		public ProviderPortfolioItem AddPortfolioItem(string title, string? description, string? imageUrl)
		{
			var item = ProviderPortfolioItem.Create(Id, title, description, imageUrl);
			_portfolio.Add(item);
			return item;
		}

		public void SetAvailability(DayOfWeek day, TimeSpan start, TimeSpan end)
		{
			if (start >= end) throw new ArgumentException("Start time must be before end time.");
			_availabilities.RemoveAll(a => a.DayOfWeek == day);
			_availabilities.Add(ProviderAvailability.Create(Id, day, start, end));
		}




		public void MarkUnderReview()
		{
			//if (VerificationStatus is VerificationStatus.Approved or VerificationStatus.Suspended)
			//	throw new DomainException("Cannot move a finalized provider back to review.");

			VerificationStatus = VerificationStatus.UnderReview;
			//AddDomainEvent(new ProviderUnderReviewDomainEvent(Id));
		}

		public void Reject(string reason)
		{
			if (string.IsNullOrWhiteSpace(reason))
				throw new ArgumentException("A rejection reason is required.", nameof(reason));

			VerificationStatus = VerificationStatus.Suspended;
			//AddDomainEvent(new ProviderRejectedDomainEvent(Id, reason));
		}
	}

	
	
}
