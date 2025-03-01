using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
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
}