namespace Backend.Application.Auth.Commands.Authenticate;

public sealed record AuthenticateCommand(
    string Username, 
    string Password);
