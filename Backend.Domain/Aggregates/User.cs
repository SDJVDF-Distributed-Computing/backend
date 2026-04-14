namespace Backend.Domain.Aggregates;

public class User : BaseEntity
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public static User Create(string username, string passwordHash)
    {
        return new User
        {
            Username = username,
            PasswordHash = passwordHash,
        };
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        SetUpdatedAt();
    }
}