namespace HRMSystemLiu.Model.Web;

public class FunctionalInput<T>
{
    public FunctionalInput(string label)
    {
        Label = label;

        if (typeof(T) == typeof(DateTime)) BindValue = (T)(object)DateTime.Today;
    }

    public string Label { get; }

    public T? BindValue { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public bool FilterEnabled { get; set; }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public T? GetValue()
    {
        return FilterEnabled ? BindValue : default;
    }
}