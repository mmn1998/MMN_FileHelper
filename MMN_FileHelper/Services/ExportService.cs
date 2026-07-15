using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using MMN_FileHelper.Models;
using QuestPDF.Fluent;
using System.Globalization;

namespace MMN_FileHelper.Services;

public class ExportService : IExportService
{
    /// <summary>
    /// It does'nt work in persian(UTF-8). 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] ExportToCsv<T>(IEnumerable<T> data)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

        var properties = typeof(T).GetProperties();

        // Write header
        foreach (var prop in properties)
        {
            csv.WriteField(prop.Name);
        }
        csv.NextRecord();

        // Write rows
        foreach (var item in data)
        {
            foreach (var prop in properties)
            {
                csv.WriteField(prop.GetValue(item, null));
            }
            csv.NextRecord();
        }

        writer.Flush();
        return memoryStream.ToArray();
    }
    /// <summary>
    /// works perfectly
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] ExportToExcel<T>(IEnumerable<T> data)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        var properties = typeof(T).GetProperties();

        // Write header
        for (int i = 0; i < properties.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = properties[i].Name;
        }

        // Write rows
        int row = 2;
        foreach (var item in data)
        {
            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cell(row, col + 1).Value = properties[col].GetValue(item)?.ToString();
            }
            row++;
        }

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }
    public byte[] ExportToPdf<T>(IEnumerable<T> data, PdfConfigurations pdfConfigurations)
    {
        var pdfDoc = Document.Create(document =>
        {
            document.Page(page =>
            {
                //page.ContentFromRightToLeft();
                page.Margin(pdfConfigurations.PageMargin);
                page.Header()
                    .Text(pdfConfigurations.HeaderText)
                    .FontFamily(pdfConfigurations.FontFamily).Fallback()
                    .SemiBold();

                page.Content().AlignCenter()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(cd =>
                        {
                            for (int i = 0; i < pdfConfigurations.ColumnsCount; i++)
                            {
                                cd.RelativeColumn(pdfConfigurations.ColumnWidths);
                            }
                        });

                        table.Header(tableHeader =>
                        {
                            foreach (var colunName in pdfConfigurations.ColumnNames)
                            {
                                tableHeader.Cell()
                                    .Border(pdfConfigurations.TableBorders)
                                    .AlignCenter()
                                    .Text(colunName)
                                    .FontFamily(pdfConfigurations.FontFamily)
                                    .Fallback();
                            }
                        });
                        var properties = typeof(T).GetProperties();
                        foreach (var item in data)
                        {
                            foreach (var columnName in pdfConfigurations.ColumnNames)
                            {
                                var prop = properties.First(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                                var value = prop.GetValue(item)?.ToString() ?? string.Empty;

                                table.Cell()
                                    .Border(pdfConfigurations.TableBorders)
                                    .AlignCenter()
                                    .Text(value)
                                    .FontFamily(pdfConfigurations.FontFamily)
                                    .Fallback();
                            }
                        }
                    });
                page.PageColor(pdfConfigurations.PageBackgroundColor);
                page.Footer()
                    .Text(text =>
                    {
                        text.CurrentPageNumber();
                    });
            });
        });
        return pdfDoc.GeneratePdf();
    }
}