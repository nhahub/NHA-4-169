using BayTack.Domain.Common;
using System;
using System.Collections.Generic;

namespace BayTack.Domain.ValueObjects
{
    /// <summary>One pricing tier of a Service (basic/standard/premium) - mirrors the
    /// packages shape in Front_end's service mock (name/price/desc/delivery).</summary>
    public sealed class ServicePackage : ValueObject
    {
        public string Name { get; private set; }
        public Money Price { get; private set; }
        public string Description { get; private set; }
        public string DeliveryEstimate { get; private set; }

        private ServicePackage()
        {
            Name = string.Empty;
            Price = Money.Zero();
            Description = string.Empty;
            DeliveryEstimate = string.Empty;
        }

        private ServicePackage(string name, Money price, string description, string deliveryEstimate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Package name is required.", nameof(name));

            Name = name;
            Price = price;
            Description = description;
            DeliveryEstimate = deliveryEstimate;
        }

        public static ServicePackage Create(string name, Money price, string description, string deliveryEstimate) =>
            new(name, price, description, deliveryEstimate);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Price;
            yield return Description;
            yield return DeliveryEstimate;
        }
    }
}