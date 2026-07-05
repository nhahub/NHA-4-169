using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Common.BaseEntity
{
	public abstract class BaseEntity<TId>
	{
		public TId Id { get; protected set; } = default!;

		private readonly List<IDomainEvent> _domainEvents = new();
		public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

		protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
		public void ClearDomainEvents() => _domainEvents.Clear();

		public override bool Equals(object? obj)
		{
			if (obj is not BaseEntity<TId> other) return false;
			if (ReferenceEquals(this, other)) return true;
			if (GetType() != other.GetType()) return false;
			return EqualityComparer<TId>.Default.Equals(Id, other.Id);
		}

		public override int GetHashCode() => HashCode.Combine(GetType(), Id);
	}

}
