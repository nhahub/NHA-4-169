using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Entities.PaymentAggregate;
using BayTack.Domain.ValueObjects;

namespace BayTack.Domain.Entities.ServiceAggregate
{

	public class Service : AuditableEntity<string>
	{
		public string CategoryId { get; private set; }
		public string Name { get; private set; } = string.Empty;
		public string? Description { get; private set; }
		public Money MinPrice { get; private set; } = Money.Zero();
		public Money MaxPrice { get; private set; } = Money.Zero();
		public bool AllowCredit { get; private set; }
		public bool AllowInstallments { get; private set; }

		private readonly List<ServicePaymentMethod> _paymentMethods = new();
		public IReadOnlyCollection<ServicePaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

		private Service() { }

		public static Service Create(string categoryId, string name, Money minPrice, Money maxPrice,
			bool allowCredit = false, bool allowInstallments = false, string? description = null)
		{
			if (minPrice.Amount > maxPrice.Amount)
				throw new ArgumentException("Min price cannot exceed max price.");

			return new Service
			{
				CategoryId = categoryId,
				Name = name,
				Description = description,
				MinPrice = minPrice,
				MaxPrice = maxPrice,
				AllowCredit = allowCredit,
				AllowInstallments = allowInstallments
			};
		}

		public void AllowPaymentMethod(string paymentMethodId) =>
			_paymentMethods.Add(ServicePaymentMethod.Create(Id, paymentMethodId));
	}

}
