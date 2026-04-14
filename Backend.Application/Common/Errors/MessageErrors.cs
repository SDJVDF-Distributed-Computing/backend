namespace Backend.Application.Common.Errors;

public static class MessageErrors
{
    public static readonly Error InvalidMessageContent =
        new("messages.content.invalid", "The provided message content is not valid.");
}