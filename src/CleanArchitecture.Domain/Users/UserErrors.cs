using ErrorOr;

namespace CleanArchitecture.Domain.Users;

public static class UserErrors
{
    public static Error SubscriptionNotFound => Error.NotFound(
        code: "User.SubscriptionNotFound",
        description: "Subscription not found");

    public static Error ReminderNotFound => Error.NotFound(
        code: "User.ReminderNotFound",
        description: "Reminder not found");

    public static Error ReminderAlreadyDismissed => Error.Conflict(
        code: "User.ReminderAlreadyDismissed",
        description: "Reminder already dismissed");

    public static Error CannotCreateMoreRemindersThanSubscriptionAllows => Error.Conflict(
        code: "User.ReminderLimitExceeded",
        description: "You have reached the daily reminder limit for your subscription.");
}
