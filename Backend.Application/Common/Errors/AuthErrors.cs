namespace Backend.Application.Common.Errors;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials =
        new("auth.credentials.invalid", "Username or password is incorrect.");

    public static readonly Error InvalidServerAddress =
        new("auth.server_address.invalid", "Host or port is incorrect.");

    public static readonly Error NotConnected =
        new("auth.not_connected", "No active server connection.");

    public static readonly Error NotAuthenticated =
        new("auth.not_authenticated", "Session is not authenticated.");
}