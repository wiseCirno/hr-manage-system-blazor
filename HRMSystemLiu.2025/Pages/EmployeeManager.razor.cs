using System.Data;
using System.Text;
using HRMSystemLiu.DAL;
using HRMSystemLiu.Model;
using HRMSystemLiu.Model.Database;
using HRMSystemLiu.Model.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace HRMSystemLiu._2025.Pages;

public partial class EmployeeManager : ComponentBase
{
    #region Lifecycle Methods

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_helper.HasLoadedOnInit)
        {
            _helper.HasLoadedOnInit = true;
            await PanelInit();
        }
    }

    #endregion

    #region Event Handlers & UI Helpers

    private void OnSearchEmployeeFocus()
    {
        _nameSearch.ErrorMessage = string.Empty;
        _dateToSearch.ErrorMessage = string.Empty;
    }

    #endregion

    #region Fields and Properties

    private readonly SettingHelper _helper = new("general");

    private string _avatarUrl = ConstProperty.DefaultAvatarUrl;
    private List<Department> _departmentList = [];
    private Employee _editedEmployee = new();
    private List<EmployeeLite>? _employeesLite;
    private DictionaryList _employeeIdList = new();
    private DataTable? _employeeTable;
    private IJSRuntime _jsRuntime = null!;

    private Employee _newEmployee = new();
    private Guid _selectedEmployeeId = Guid.Empty;
    private readonly FunctionalInput<string> _nameSearch = new("Name");
    private readonly FunctionalInput<Guid> _departmentSearch = new("Department");
    private readonly FunctionalInput<DateTime> _dateFromSearch = new("From Date");
    private readonly FunctionalInput<DateTime> _dateToSearch = new("To Date");
    private readonly FunctionalButton _updateInfoFunctionalButton = new("Modify", "Success");
    private readonly FunctionalButton _updateResumeRemarkFunctionalButton = new("Modify", "Success");

    #endregion

    #region Initialization & Panel Switching

    private async Task PanelInit()
    {
        switch (_helper.ActivePanel)
        {
            case "general":
                if (_employeeTable == null) await LoadEmployeeAsync();
                ResetAvatar();
                break;
            case "manage":
                _selectedEmployeeId = Guid.Empty;
                if (_employeesLite == null) await LoadEmployeeLiteAsync();
                ResetAvatar();
                break;
        }
    }

    private async Task SwitchPanel(string name)
    {
        await _helper.SwitchPanelAsync(name);
        StateHasChanged();
    }

    private void ResetAvatar()
    {
        _avatarUrl = ConstProperty.DefaultAvatarUrl;
    }

    #endregion

    #region Employee CRUD Operations

    private async Task OnEmployeeSelected(Guid id)
    {
        _selectedEmployeeId = id;
        _editedEmployee = await EmployeeService.GetEmployeeAsync(_selectedEmployeeId);
        UpdateAvatarFromEmployee(_editedEmployee);
    }

    private async Task AvatarHandler(InputFileChangeEventArgs e, Employee employee, bool immediateCommit = false)
    {
        if (e.FileCount == 0) return;

        const long maxFileSize = 5 * 1024 * 1024;
        using var ms = new MemoryStream();
        await e.File.OpenReadStream(maxFileSize).CopyToAsync(ms);
        employee.Photo = ms.ToArray();

        if (immediateCommit) await EmployeeService.UpdateEmployeeAsync(employee);

        _avatarUrl = GetImageUrl(e.File.ContentType, employee.Photo);
        StateHasChanged();
    }

    private async Task AddEmployeeAsync()
    {
        if (await EmployeeService.AddEmployeeAsync(_newEmployee))
        {
            _newEmployee = new Employee();
            await LoadEmployeeAsync();
            ResetAvatar();
        }
    }

    private async Task DeleteEmployeeAsync()
    {
        if (await EmployeeService.DeleteEmployeeAsync(_selectedEmployeeId))
        {
            _selectedEmployeeId = Guid.Empty;
            await LoadEmployeeLiteAsync();
            _editedEmployee = new Employee();
        }
    }

    private async Task LoadEmployeeAsync()
    {
        if (IsLoading()) return;
        SetLoading(true);

        try
        {
            _employeeTable = await EmployeeService.GetEmployeeAsync();
            _departmentList = await DepartmentService.GetDepartmentIdDictionaryAsync();
            _employeeIdList = await DictionaryService.GetDictionarySheetAsync();
        }
        finally
        {
            SetLoading(false);
            StateHasChanged();
        }
    }

    private async Task LoadEmployeeLiteAsync()
    {
        if (IsLoading()) return;
        SetLoading(true);

        try
        {
            _employeesLite = await EmployeeService.GetEmployeeLiteAsync();
        }
        finally
        {
            SetLoading(false);
            StateHasChanged();
        }
    }

    private async Task FilterEmployeeAsync()
    {
        if (_nameSearch.FilterEnabled && string.IsNullOrWhiteSpace(_nameSearch.BindValue))
        {
            _nameSearch.ErrorMessage = "Invalid employee name.";
            return;
        }

        if (_dateFromSearch.FilterEnabled && _dateToSearch.FilterEnabled &&
            _dateFromSearch.BindValue > _dateToSearch.BindValue)
        {
            _dateToSearch.ErrorMessage = "Invalid date range.";
            return;
        }

        if (IsLoading()) return;
        SetLoading(true);

        try
        {
            var condition = new SearchCondition
            {
                SearchText = _nameSearch.GetValue(),
                DepartmentId = _departmentSearch.GetValue(),
                FromDate = _dateFromSearch.GetValue(),
                ToDate = _dateToSearch.GetValue()
            };

            _employeeTable = await EmployeeService.GetEmployeeAsync(condition);
        }
        finally
        {
            SetLoading(false);
            StateHasChanged();
        }
    }

    private async Task ModifyEmployeeAsync(FunctionalButton flag)
    {
        if (await EmployeeService.UpdateEmployeeAsync(_editedEmployee))
        {
            flag.StateChanged = true;
            StateHasChanged();
            await Task.Delay(1000);
            flag.StateChanged = false;
            StateHasChanged();
        }
    }

    private async Task ExportEmployeeAsync()
    {
        if (_employeeTable == null) return;

        var csvContent = BuildCsvContent(_employeeTable);
        var fileName = $"EmployeeExport_{DateTime.Now:yyyyMMddHHmmss}.csv";
        var bytes = Encoding.UTF8.GetBytes(csvContent);

        await _jsRuntime.InvokeVoidAsync("downloadFileFromBytes", fileName, "text/csv", bytes);
    }

    #endregion

    #region Helper Methods

    private bool IsLoading()
    {
        return _helper.IsLoading;
    }

    private void SetLoading(bool loading)
    {
        _helper.IsLoading = loading;
    }

    private void UpdateAvatarFromEmployee(Employee employee)
    {
        if (employee.Photo != null)
            _avatarUrl = GetImageUrl(CommonHelper.GetImageContentType(employee.Photo), employee.Photo);
        else
            ResetAvatar();
    }

    private static string GetImageUrl(string contentType, byte[] photo)
    {
        return $"data:{contentType};base64,{Convert.ToBase64String(photo)}";
    }

    private static string BuildCsvContent(DataTable table)
    {
        var csvBuilder = new StringBuilder();

        var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName);
        csvBuilder.AppendLine(string.Join(",", columnNames));

        foreach (DataRow row in table.Rows)
        {
            var fields = row.ItemArray.Select(field => field?.ToString()?.Replace("\"", "\"\""));
            csvBuilder.AppendLine(string.Join(",", fields));
        }

        return csvBuilder.ToString();
    }

    #endregion
}