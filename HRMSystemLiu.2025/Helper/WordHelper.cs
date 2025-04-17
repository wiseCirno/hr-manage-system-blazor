using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HRMSystemLiu.Model.Database;

namespace HRMSystemLiu._2025.Helper;

public static class WordHelper
{
    public static byte[] GenerateSalaryReportWord(List<SalaryReportItem> salaryReportItems)
    {
        if (salaryReportItems == null || salaryReportItems.Count == 0)
            throw new ArgumentException("Salary report items list is null or empty.", nameof(salaryReportItems));

        using var stream = new MemoryStream();
        using (var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
        {
            var mainPart = wordDocument.AddMainDocumentPart();
            mainPart.Document = new Document(new Body());

            var body = mainPart.Document.Body;

            var titleParagraph = CreateParagraph("工资报表", true, 32, JustificationValues.Center);
            body!.AppendChild(titleParagraph);

            body.AppendChild(new Paragraph(new Run(new Break())));

            var table = new Table();
            var tblProps = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                )
            );
            table.AppendChild(tblProps);
            var headers = new[] { "序号", "姓名", "基本工资", "奖金", "罚款", "其他", "合计" };
            var headerRow = new TableRow();
            foreach (var header in headers)
            {
                var tc = new TableCell(new Paragraph(new Run(new Text(header)))
                {
                    ParagraphProperties =
                        new ParagraphProperties(new Justification { Val = JustificationValues.Center })
                });
                tc.Append(new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                headerRow.Append(tc);
            }

            table.Append(headerRow);

            foreach (var item in salaryReportItems)
            {
                var dataRow = new TableRow();
                // 序号
                dataRow.Append(CreateTableCell(item.SerialNumber.ToString()));
                // 姓名
                dataRow.Append(CreateTableCell(item.Name));
                // 基本工资
                dataRow.Append(CreateTableCell(item.BaseSalary.ToString("N2", CultureInfo.InvariantCulture),
                    JustificationValues.Right));
                // 奖金
                dataRow.Append(CreateTableCell(item.Bonus.ToString("N2", CultureInfo.InvariantCulture),
                    JustificationValues.Right));
                // 罚款
                dataRow.Append(CreateTableCell(item.Fine.ToString("N2", CultureInfo.InvariantCulture),
                    JustificationValues.Right));
                // 其他
                dataRow.Append(CreateTableCell(item.Other.ToString("N2", CultureInfo.InvariantCulture),
                    JustificationValues.Right));
                // 合计
                dataRow.Append(CreateTableCell(item.Total.ToString("N2", CultureInfo.InvariantCulture),
                    JustificationValues.Right));

                table.Append(dataRow);
            }

            body.Append(table);
            mainPart.Document.Save();
        }

        return stream.ToArray();
    }

    private static Paragraph CreateParagraph(string text, bool bold, int fontSizeHalfPt,
        JustificationValues justification)
    {
        var runProps = new RunProperties
        {
            FontSize = new FontSize { Val = fontSizeHalfPt.ToString() }
        };

        if (bold) runProps.Bold = new Bold();
        var run = new Run(runProps, new Text(text) { Space = SpaceProcessingModeValues.Preserve });
        var paraProps = new ParagraphProperties(new Justification { Val = justification });
        var para = new Paragraph(paraProps);
        para.Append(run);

        return para;
    }

    private static TableCell CreateTableCell(string text)
    {
        return CreateTableCell(text, JustificationValues.Left);
    }

    private static TableCell CreateTableCell(string text, JustificationValues justification)
    {
        var para = new Paragraph(new Run(new Text(text)))
        {
            ParagraphProperties = new ParagraphProperties(new Justification { Val = justification })
        };

        var tc = new TableCell(para);
        tc.AppendChild(new TableCellProperties(
            new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2400" }
        ));
        return tc;
    }
}