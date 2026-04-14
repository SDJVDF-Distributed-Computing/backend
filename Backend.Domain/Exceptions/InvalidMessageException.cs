namespace Backend.Domain.Exceptions;

public sealed class InvalidMessageException : DomainException
{
    public IReadOnlyList<string> Violations { get; }

    public InvalidMessageException(IReadOnlyList<string> violations)
        : base($"Message does not meet requirements: {string.Join(", ", violations)}.")
    {
        Violations = violations;
    }
}