using HRMSystemLiu.DAL;

namespace HRMSystemLiu.BLL;

public class RegisterUser
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RealName { get; set; } = string.Empty;

    public async Task<RegisterType> Register()
    {
        var succeed = await OperatorService.CreateOperatorAsync(UserName, Password, RealName);

        if (succeed == null) return RegisterType.Existed;

        return succeed == false ? RegisterType.Failed : RegisterType.Success;
    }
}

public enum RegisterType
{
    Success,
    Existed,
    Failed
}