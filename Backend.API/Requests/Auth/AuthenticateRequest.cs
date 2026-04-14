namespace Backend.API.Requests.Auth;

public sealed record AuthenticateRequest(string Username, string Password);
