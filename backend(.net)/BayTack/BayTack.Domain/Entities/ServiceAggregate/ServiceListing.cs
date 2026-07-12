using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.ValueObjects;
using System;

namespace BayTack.Domain.Entities.ServiceAggregate
{
    /// <summary>A specific provider's offering of a catalog Service - this is what
    /// customers actually browse/book (Front_end/customer/app/browse, service/, saved/).
    /// Service itself stays a category-level template (name, description, a rough
    /// min/max price range); ServiceListing is where a real ProviderId and the 3 concrete
    /// pricing tiers live, so Service doesn't have to be touched for this.</summary>
    public class ServiceListing : AuditableEntity<string>
    {
        public string ServiceId { get; private set; } = string.Empty;
        public string ProviderId { get; private set; } = string.Empty;

        /// <summary>Material icon name (Front_end uses Material Symbols, e.g.
        /// "cleaning_services") - purely a display hint, no domain meaning.</summary>
        public string? IconName { get; private set; }

        public ServicePackage BasicPackage { get; private set; } = null!;
        public ServicePackage StandardPackage { get; private set; } = null!;
        public ServicePackage PremiumPackage { get; private set; } = null!;

        private ServiceListing() { }

        public static ServiceListing Create(
            string serviceId, string providerId,
            ServicePackage basicPackage, ServicePackage standardPackage, ServicePackage premiumPackage,
            string? iconName = null)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Service id is required.", nameof(serviceId));
            if (string.IsNullOrWhiteSpace(providerId))
                throw new ArgumentException("Provider id is required.", nameof(providerId));

            return new ServiceListing
            {
                // Same PK-generation gap flagged in Messaging/Conversation.cs.
                Id = Guid.NewGuid().ToString(),
                ServiceId = serviceId,
                ProviderId = providerId,
                BasicPackage = basicPackage,
                StandardPackage = standardPackage,
                PremiumPackage = premiumPackage,
                IconName = iconName
            };
        }

        public void UpdatePricing(ServicePackage basicPackage, ServicePackage standardPackage, ServicePackage premiumPackage)
        {
            BasicPackage = basicPackage;
            StandardPackage = standardPackage;
            PremiumPackage = premiumPackage;
            SetUpdated(ProviderId);
        }

        public void SetIcon(string? iconName) => IconName = iconName;

        public ServicePackage PackageFor(string? tier) => tier switch
        {
            "basic" => BasicPackage,
            "premium" => PremiumPackage,
            _ => StandardPackage // "standard" or unspecified
        };
    }
}