namespace BayTack.Domain.Common
{
	/// <summary>
	/// Marker interface for domain events raised by aggregates via BaseEntity.AddDomainEvent.
	/// Nothing currently dispatches these (no MediatR INotification publishing is wired up
	/// yet) — this only existed as a dangling reference in BaseEntity.cs before, so the
	/// Domain project wasn't compiling at all. Wire up a dispatcher (e.g. in
	/// UnitOfWork.SaveChangesAsync, publish each entity's DomainEvents via IPublisher
	/// before/after saving) when domain events are actually needed.
	/// </summary>
	public interface IDomainEvent
	{
	}
}
