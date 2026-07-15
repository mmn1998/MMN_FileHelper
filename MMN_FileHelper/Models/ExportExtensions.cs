namespace MMN_FileHelper.Models;

public class ExportExtensions
{
    public static string ExcelExtension { get; } = "xlsx";
    public static string CsvExtension { get; } = "csv";
    public static string PdfExtension { get; } = "pdf";
}
public class ExportContentTypes
{
    public static string ExcelContentType { get; } = "application/vnd.ms-excel";
    public static string CsvContentType { get; } = "text/cs";
    public static string PdfContentType { get; } = "application/pdf";
}
