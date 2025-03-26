using HRMSystemLiu.DAL;
using HRMSystemLiu.Model.Database;

namespace HRMSystemLiu.BLL;

public class LoginUser
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public async Task<LoginType> Login()
    {
        var op = await OperatorService.GetOperatorAsync(UserName);
        LoginType loginResult;
        string desc;

        if (op == null)
        {
            loginResult = LoginType.NotFound;
            desc = $"Try to login as {UserName} but the user does not exist.";
        }
        else if (op.IsDeleted)
        {
            loginResult = LoginType.Deleted;
            desc = $"Try to login as {UserName} but the user is deleted.";
        }
        else if (op.IsLocked)
        {
            loginResult = LoginType.Locked;
            desc = $"Try to login as {UserName} but the user is locked.";
        }
        else if (op.Password != CommonHelper.Md5EncryptI32(Password))
        {
            loginResult = LoginType.Fail;
            desc = $"Try to login as {UserName} but the password does not match.";
        }
        else
        {
            if (op.IsAdmin)
            {
                loginResult = LoginType.Admin;
                desc = $"Admin {UserName} logged in.";
            }
            else
            {
                loginResult = LoginType.User;
                desc = $"User {UserName} logged in.";
            }
        }

        var log = new OperationLog
        {
            OperatorId = op?.Id ?? Guid.Empty,
            ActionDesc = desc
        };

        if (!await OperationLogService.AddOperationLogAsync(log))
            throw new OperationCanceledException("Insert operation failed.");

        return loginResult;
    }
}

public enum LoginType
{
    User, // Login
    Admin,
    Locked,
    Deleted,
    Fail,
    NotFound // Additional Type
}