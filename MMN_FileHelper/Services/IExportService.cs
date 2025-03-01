namespace MMN_FileHelper.Services;

public interface IExportService
{
    byte[] ExportToExcel<T>(IEnumerable<T> data);
    byte[] ExportToCsv<T>(IEnumerable<T> data);
}