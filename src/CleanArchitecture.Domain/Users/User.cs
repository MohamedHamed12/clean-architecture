using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Reminders;
using CleanArchitecture.Domain.Subscriptions;
using CleanArchitecture.Domain.Users.Events;
using ErrorOr;
using Throw;

namespace CleanArchitecture.Domain.Users;

public class User : Entity
{
    private readonly Calendar _calendar = null!;
    private readonly List<Guid> _reminderIds = [];
    private readonly List<Guid> _dismissedReminderIds = [];

    public Subscription Subscription { get; private set; } = Subscription.Canceled;
    public string Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    private string _hashedPassword = null!;

    public User(
        Guid id,
        string firstName,
        string lastName,
        string email,
        Subscription? subscription = null,
        Calendar? calendar = null)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Subscription = subscription ?? Subscription.Canceled;
        _calendar = calendar ?? Calendar.Empty();
    }

    public ErrorOr<Success> SetReminder(Reminder reminder)
    {
        if (Subscription == Subscription.Canceled)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        reminder.SubscriptionId.Throw().IfNotEquals(Subscription.Id);

        if (HasReachedDailyReminderLimit(reminder.DateTime))
        {
            return UserErrors.CannotCreateMoreRemindersThanSubscriptionAllows;
        }

        _calendar.IncrementEventCount(reminder.Date);
        _reminderIds.Add(reminder.Id);

        AddDomainEvent(new ReminderSetEvent(reminder));
        return Result.Success;
    }

    public ErrorOr<Success> DismissReminder(Guid reminderId)
    {
        if (Subscription == Subscription.Canceled)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        if (!_reminderIds.Contains(reminderId))
        {
            return Error.NotFound(description: "Reminder not found");
        }

        if (_dismissedReminderIds.Contains(reminderId))
        {
            return Error.Conflict(description: "Reminder already dismissed");
        }

        _dismissedReminderIds.Add(reminderId);
        AddDomainEvent(new ReminderDismissedEvent(reminderId));

        return Result.Success;
    }

    public ErrorOr<Success> DeleteReminder(Reminder reminder)
    {
        if (Subscription == Subscription.Canceled)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        if (!_reminderIds.Remove(reminder.Id))
        {
            return Error.NotFound(description: "Reminder not found");
        }

        _dismissedReminderIds.Remove(reminder.Id);
        _calendar.DecrementEventCount(reminder.Date);

        AddDomainEvent(new ReminderDeletedEvent(reminder.Id));
        return Result.Success;
    }

    public void DeleteAllReminders()
    {
        foreach (var reminderId in _reminderIds)
        {
            AddDomainEvent(new ReminderDeletedEvent(reminderId));
        }

        _reminderIds.Clear();
        _dismissedReminderIds.Clear();
    }

    public ErrorOr<Success> CancelSubscription(Guid subscriptionId)
    {
        if (Subscription.Id != subscriptionId)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        Subscription = Subscription.Canceled;
        AddDomainEvent(new SubscriptionCanceledEvent(this, subscriptionId));

        return Result.Success;
    }

    public void SetPassword(string password)
    {
        // Hash logic placeholder — replace with proper hashing logic or a password service
        _hashedPassword = HashPassword(password);
    }

    private static string HashPassword(string password)
    {
        // Simplified for demonstration — never use plain hashing like this in real projects!
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private bool HasReachedDailyReminderLimit(DateTimeOffset dateTime)
    {
        var dailyCount = _calendar.GetNumEventsOnDay(dateTime.Date);
        var limit = Subscription.SubscriptionType.GetMaxDailyReminders();

        return dailyCount >= limit || dailyCount == int.MaxValue;
    }

    // EF Core or ORM-friendly constructor
    private User() { }
}
