namespace Backend.Domain.Exceptions;

public sealed class SessionNotAuthenticatedException : DomainException
{
    public SessionNotAuthenticatedException()
        : base("Session must be authenticated before performing UPLD/DNLD operations.") { }
}
