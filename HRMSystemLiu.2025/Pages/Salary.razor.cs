using HRMSystemLiu._2025.Helper;
using HRMSystemLiu.DAL;
using HRMSystemLiu.Model.Database;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HRMSystemLiu._2025.Pages;

public partial class Salary : ComponentBase
{
    #region Fields and Properties
    
    private readonly SettingHelper _helper = new("general");
    private readonly SalarySheet _previewSalarySheet = new();
    private List<Department> _departmentList = [];
    private bool _existedSalarySheet;
    private List<Model.Database.Employee> _filteredEmployees = [];
    private IJSRuntime _jsRuntime = null!;
    private SalarySheet _newSalarySheet = new();
    private List<SalarySheetItem> _newSalarySheetItems = [];
    private bool _notCreated = true;
    private List<SalaryReportItem> _salaryReportItems = [];

    private List<SalarySheet> _salarySheetList = [];
    private string _sortColumn = "Year";
    private bool _sortDirection = true; // true for ascending, false for descending
    
    private List<SalarySheet> SortedSalarySheetList
    {
        get
        {
            if (string.IsNullOrEmpty(_sortColumn)) return _salarySheetList;

            return _sortDirection
                ? _salarySheetList.OrderBy(GetSortProperty).ToList()
                : _salarySheetList.OrderByDescending(GetSortProperty).ToList();
        }
    }
    
    #endregion

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
    
    #region SalarySheet Methods
    
    private void SortTable(string columnName)
    {
        if (_sortColumn != columnName)
        {
            _sortColumn = columnName;
            _sortDirection = true;
        }
        else
        {
            _sortDirection = !_sortDirection;
        }
    }
    
    private Func<SalarySheet, object> GetSortProperty
    {
        get
        {
            return _sortColumn switch
            {
                "Year" => sheet => sheet.Year,
                "Month" => sheet => sheet.Month,
                "Department" => sheet => sheet.DepartmentId,
                _ => sheet => sheet.Year
            };
        }
    }

    private async Task SalarySheetHandler()
    {
        if (_notCreated)
        {
            if (await SalarySheetService.IsExistSalarySheetAsync(_newSalarySheet))
            {
                _existedSalarySheet = true;
                return;
            }

            (_newSalarySheetItems, _filteredEmployees) =
                await SalarySheetService.CreateNewSalarySheetAsync(_newSalarySheet);
            _notCreated = false;
            StateHasChanged();
        }
        else
        {
            await SalarySheetService.UpdateSalarySheetItemsAsync(_newSalarySheetItems);
            _salarySheetList = await SalarySheetService.GetSalarySheetListAsync();
            ResetNewSalarySheet();
        }
    }

    private void ResetNewSalarySheet()
    {
        _existedSalarySheet = false;
        _notCreated = true;
        _newSalarySheet = new SalarySheet();
        _newSalarySheetItems = [];
        StateHasChanged();
    }

    private async Task OverrideExistedSalarySheet()
    {
        (_newSalarySheetItems, _filteredEmployees) = await SalarySheetService.RecreateSalarySheetAsync(_newSalarySheet);
        _notCreated = false;
        _existedSalarySheet = false;
        StateHasChanged();
    }

    private async Task GetExistedSalarySheet()
    {
        (_newSalarySheetItems, _filteredEmployees) =
            await SalarySheetService.GetExistedSalarySheetAsync(_newSalarySheet);
        _notCreated = false;
        _existedSalarySheet = false;
        StateHasChanged();
    }

    private async Task GetFilteredSalaryReport()
    {
        var sheet = await SalarySheetService.GetSalarySheetAsync(_previewSalarySheet);
        _salaryReportItems = await SalaryReportService.GetSalaryReportItems(sheet.Id);
        StateHasChanged();
    }

    private async Task DownloadSalaryReport()
    {
        if (_salaryReportItems.Count == 0)
            return;
        var department = _departmentList.FirstOrDefault(d => d.Id == _previewSalarySheet.DepartmentId)?.Name ?? "未知部门";
        var pdfContent = WordHelper.GenerateSalaryReportWord(_salaryReportItems);
        var fileName = $"SalaryReport({department}-{_previewSalarySheet.Year}-{_previewSalarySheet.Month}).docx";
        await _jsRuntime.InvokeVoidAsync("downloadFileFromBytes", fileName, "application/document", pdfContent);
    }
    
    #endregion

    #region Initialization & Panel Switching

    private async Task PanelInit()
    {
        switch (_helper.ActivePanel)
        {
            case "general":
                _departmentList = await DepartmentService.GetDepartmentIdDictionaryAsync();
                _salarySheetList = await SalarySheetService.GetSalarySheetListAsync();
                StateHasChanged();
                break;
            case "reportForm":
                _departmentList = await DepartmentService.GetDepartmentIdDictionaryAsync();
                StateHasChanged();
                break;
        }
    }

    private async Task SwitchPanel(string name)
    {
        await _helper.SwitchPanelAsync(name);
        StateHasChanged();
    }

    #endregion
}