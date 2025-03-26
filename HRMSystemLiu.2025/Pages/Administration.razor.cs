using System.Data;
using HRMSystemLiu._2025.Utils.Helper;
using HRMSystemLiu.DAL;
using Microsoft.AspNetCore.Components;

namespace HRMSystemLiu._2025.Pages;

public partial class Administration : ComponentBase
{
    private readonly SettingHelper _helper = new("department");
    private readonly int[] _logPageSelectorSizes = [5, 10, 20, 50, 100];
    private int _logCurrentPage = 1;
    private int _logCurrentPageSize = 5;
    private DataTable? _logTable;
    private int _logTotalPages;
    private string _newDepartment = string.Empty;
    private string _newDepartmentErr = string.Empty;
    private string[] _selectedDepartments = [];
    private string[] _totalDepartments = [];

    private async Task PanelInit()
    {
        switch (_helper.ActivePanel)
        {
            case "department":
                await LoadDepartmentAsync();
                break;
            case "logs":
                await LoadLogAsync();
                break;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_helper.HasLoadedOnInit)
        {
            _helper.HasLoadedOnInit = true;
            await PanelInit();
        }
    }

    private async Task SwitchPanel(string name)
    {
        await _helper.SwitchPanelAsync(name);
        StateHasChanged();
    }

    private void OnAddDepartmentFocused()
    {
        _newDepartmentErr = string.Empty;
    }

    private void OnDepartmentSelected(string dept)
    {
        _selectedDepartments = _selectedDepartments.Contains(dept)
            ? _selectedDepartments.Where(d => d != dept).ToArray()
            : _selectedDepartments.Append(dept).ToArray();
    }

    private async Task LoadDepartmentAsync()
    {
        if (_helper.IsLoading) return;
        _helper.IsLoading = true;

        try
        {
            _totalDepartments = await DepartmentService.GetDepartmentsAsync();
        }
        finally
        {
            _helper.IsLoading = false;
            StateHasChanged();
        }
    }

    private async Task AddDepartmentAsync()
    {
        if (string.IsNullOrWhiteSpace(_newDepartment))
        {
            _newDepartmentErr = "Invalid department name.";
            return;
        }

        var result = await DepartmentService.AddDepartmentAsync(_newDepartment);
        switch (result)
        {
            case null:
                _newDepartmentErr = "Can not add an existed department.";
                return;
            case false:
                _newDepartmentErr = "Unknown error.";
                return;
            default:
                await LoadDepartmentAsync();
                _newDepartment = string.Empty;
                break;
        }
    }

    private async Task DeleteDepartmentAsync()
    {
        if (await DepartmentService.DeleteDepartment(_selectedDepartments))
        {
            await LoadDepartmentAsync();
            _selectedDepartments = [];
        }
    }

    private async Task LoadLogAsync()
    {
        if (_helper.IsLoading) return;
        _helper.IsLoading = true;
        try
        {
            var service = new OperationLogService();
            _logTable = await service.GetOperationLogListAsync(_logCurrentPage, _logCurrentPageSize);
            _logTotalPages = service.TotalPages;
        }
        finally
        {
            _helper.IsLoading = false;
            StateHasChanged();
        }
    }

    private async Task UpdateLogPageAsync(int newPage)
    {
        _logCurrentPage = newPage;
        await LoadLogAsync();
    }

    private async Task OnLogPageSizeChangedAsync(int size)
    {
        _logCurrentPageSize = size;
        await LoadLogAsync();
        if (_logCurrentPage > _logTotalPages)
            await UpdateLogPageAsync(_logTotalPages);
    }

    private Task LoadLogFirstPage()
    {
        return UpdateLogPageAsync(1);
    }

    private Task LoadLogNextPage()
    {
        return UpdateLogPageAsync(_logCurrentPage < _logTotalPages ? _logCurrentPage + 1 : _logCurrentPage);
    }

    private Task LoadLogPrevPage()
    {
        return UpdateLogPageAsync(_logCurrentPage > 1 ? _logCurrentPage - 1 : _logCurrentPage);
    }

    private Task LoadLogLastPage()
    {
        return UpdateLogPageAsync(_logTotalPages);
    }
}