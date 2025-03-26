namespace HRMSystemLiu.Model.Web;

public class FunctionalButton
{
    public FunctionalButton(string name)
    {
        Name = name;
    }

    public FunctionalButton(string name, string stateChanged)
    {
        Name = name;
        StateChangedName = stateChanged;
        StateChanged = false;
    }

    public string Name { get; set; }

    public bool StateChanged { get; set; }

    public string? StateChangedName { get; set; }
}
