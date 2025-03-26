using System.Data;
using Microsoft.AspNetCore.Components;

namespace HRMSystemLiu._2025.Pages;

public class SettingHelper(string defaultPanel)
{
    public bool HasLoadedOnInit;
    public bool IsLoading = false;
    public string ActivePanel { get; private set; } = defaultPanel;
    public Func<Task>? LoadCurrentPanelDataAsync { get; set; }

    public bool IsActivePanel(string panelName)
    {
        return ActivePanel == panelName;
    }

    public async Task SwitchPanelAsync(string panelName)
    {
        if (ActivePanel == panelName) return;

        ActivePanel = panelName;
        await LoadCurrentPanelDataAsync!();
    }

    public MarkupString RenderTable(DataTable? table)
    {
        if (table == null)
            return new MarkupString(
                """
                <div class="card__loading-overlay">
                    <p class="card__loading-overlay-text">
                        Loading...
                    </p>
                </div>
                """);
        
        var headerRow = $"<tr>{string.Join("", table.Columns.Cast<DataColumn>()
            .Select(col => $"""<th class="table__header-cell">{col.ColumnName}</th>"""))}</tr>";
        var thead = $"""<thead class="table__header">{headerRow}</thead>""";
        var bodyRows = table.Rows.Cast<DataRow>().Select(row =>
        {
            var rowCells = string.Join("", table.Columns.Cast<DataColumn>()
                .Select(col => $"""<td class="table__body-cell">{row[col.ColumnName]}</td>"""));
            return $"<tr>{rowCells}</tr>";
        });
        var tbody = $"<tbody>{string.Join(Environment.NewLine, bodyRows)}</tbody>";
        return new MarkupString("""<table class="table">""" + thead + Environment.NewLine + tbody + "</table>");
    }
}