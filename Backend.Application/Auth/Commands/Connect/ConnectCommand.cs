namespace Backend.Application.Auth.Commands.Connect;

public sealed record ConnectCommand(
    string Host, 
    int Port);
