namespace HRMSystemLiu.Model.Database;

public sealed class LoginUserInfo
{
    private static readonly Lazy<LoginUserInfo> _instance = new(() => new LoginUserInfo());

    private LoginUserInfo() { }

    public static LoginUserInfo Instance => _instance.Value;

    public string? RealName { get; private set; }
    public bool IsAdmin { get; private set; }

    public void InitMember(string realName, bool isAdmin)
    {
        RealName = realName;
        IsAdmin = isAdmin;
    }
}
