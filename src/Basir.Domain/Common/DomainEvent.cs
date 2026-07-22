namespace Basir.Domain.Common;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}

public abstract record DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record UserRegisteredDomainEvent(Guid UserId, string Email) : DomainEvent;

public sealed record UserLoggedInDomainEvent(Guid UserId) : DomainEvent;

public sealed record UserEmailConfirmedDomainEvent(Guid UserId) : DomainEvent;

public sealed record UserPasswordResetDomainEvent(Guid UserId) : DomainEvent;

public sealed record UserLockedOutDomainEvent(Guid UserId, DateTimeOffset LockoutEnd) : DomainEvent;
