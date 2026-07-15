using QuestPDF.Helpers;

namespace MMN_FileHelper.Models;

public class PdfConfigurations
{
    public PdfConfigurations(string pdfName, int columnsCount, List<string> columnNames, string headerText)
    {
        if (columnNames.Count != columnsCount)
            throw new Exception("Column counts and column names are not match!");
        PdfName = pdfName;
        ColumnsCount = columnsCount;
        ColumnNames = columnNames;
        HeaderText = headerText;
    }
    public int ColumnsCount { get; init; }
    public List<string> ColumnNames { get; init; }
    public int PageMargin { get; } = 10;
    public float TableBorders { get; } = 0.5f;
    public string FontFamily { get; } = "Arial";
    public int ColumnWidths { get; } = 200;
    public string PageBackgroundColor { get; } = Colors.Grey.Lighten3;
    public string HeaderText { get; init; }
    public string PdfName { get; init; }
}
