namespace Backend.Domain.Aggregates;

public class User: BaseEntity
{
    public string Username { get; private set; } = string.Empty;    
    public string Password { get; private set; } = string.Empty;

    public static User Create(
        string username,
        string password)
    {
        return new User
        {
            Username = username,
            Password = password,
        };
    }
}