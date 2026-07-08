using System;

namespace BayTack.Domain.Common.Events
{
    /// <summary>
    /// Marker for domain events raised by aggregates via BaseEntity&lt;TId&gt;.AddDomainEvent(...).
    /// This type was referenced by BaseEntity but didn't exist anywhere in the solution - the
    /// project wouldn't build at all without it. Adding the minimal interface now (no outbox /
    /// MediatR publishing wired to it yet, since no aggregate in this codebase raises an event
    /// yet) rather than building the full outbox pipeline described in the project's original
    /// design notes, which is a separate, larger piece of work.
    /// </summary>
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}