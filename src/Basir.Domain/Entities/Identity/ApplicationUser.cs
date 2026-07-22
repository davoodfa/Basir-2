using Microsoft.AspNetCore.Identity;
using Basir.Domain.Common;
using Basir.Domain.Enums;
using Basir.Domain.ValueObjects;

namespace Basir.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser<Guid>, IAuditable
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserStatus Status { get; set; } = UserStatus.Pending;
    public DateTime? LastLoginAt { get; set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();

    public Result SetEmail(Email email)
    {
        Guard.NotNull(email, nameof(email));
        Email = email.Value;
        NormalizedEmail = email.NormalizedValue;
        return Result.Success();
    }

    public Result SetPhoneNumber(PhoneNumber phoneNumber)
    {
        Guard.NotNull(phoneNumber, nameof(phoneNumber));
        PhoneNumber = phoneNumber.Value;
        return Result.Success();
    }

    public Result RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;

        if (Status is UserStatus.Pending or UserStatus.Locked)
        {
            Status = UserStatus.Active;
        }

        RaiseDomainEvent(new UserLoggedInDomainEvent(Id));
        return Result.Success();
    }

    public Result RecordFailedLoginAttempt(int maxFailedAttempts)
    {
        AccessFailedCount++;

        if (AccessFailedCount >= maxFailedAttempts)
        {
            Status = UserStatus.Locked;
            LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(15);
            RaiseDomainEvent(new UserLockedOutDomainEvent(Id, LockoutEnd.Value));
        }

        return Result.Success();
    }

    public Result ConfirmEmail()
    {
        EmailConfirmed = true;

        if (Status == UserStatus.Pending)
        {
            Status = UserStatus.Active;
        }

        RaiseDomainEvent(new UserEmailConfirmedDomainEvent(Id));
        return Result.Success();
    }

    public Result Lock(DateTimeOffset? lockoutEnd = null)
    {
        Status = UserStatus.Suspended;
        LockoutEnd = lockoutEnd ?? DateTimeOffset.MaxValue;
        return Result.Success();
    }

    public Result Unlock()
    {
        Status = UserStatus.Active;
        LockoutEnd = null;
        AccessFailedCount = 0;
        return Result.Success();
    }

    public bool CanLogin()
    {
        return Status == UserStatus.Active
            && EmailConfirmed
            && (LockoutEnd is null || LockoutEnd <= DateTimeOffset.UtcNow);
    }
}
