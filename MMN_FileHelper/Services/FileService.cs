using Microsoft.AspNetCore.Http;
using MMN_FileHelper.Extensions;
using System.Globalization;
using System.Text;

namespace SIMA.Framework.Common.Helper.FileHelper;

public class FileService : IFileService
{
    public async Task<string> Upload(IFormFile file, string rootPath)
    {
        string path = Path.Combine(rootPath, "wwwroot");
        path = Path.Combine(path, "UserUploadFiles");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileName = GetCurrentPersianTimestamp() + file.FileName;
        var filePath = Path.Combine(path, fileName);
        using (var stream = File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }
        return filePath;
    }
    public async Task<string> Upload(byte[] fileContent, string filename, string rootPath)
    {

        string path = Path.Combine(rootPath, "wwwroot");
        path = Path.Combine(path, "UserUploadFiles");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileName = GetCurrentPersianTimestamp() + filename;
        var filePath = Path.Combine(path, fileName);
        using (var stream = new MemoryStream(fileContent))
        {
            using (var filestream = System.IO.File.Create(filePath))
            {
                await stream.CopyToAsync(filestream);
            }
        }
        return filePath;
    }
    public string GetMimeType(byte[] fileBytes)
    {
        if (fileBytes.Length < 4)
            return "application/octet-stream"; // Unknown type

        // Check file signatures (magic numbers)
        if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8) return ContentTypeExtension.Mappings["jpg"];  // JPEG
        if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47) return ContentTypeExtension.Mappings["png"]; // PNG
        if (fileBytes[0] == 0x25 && fileBytes[1] == 0x50 && fileBytes[2] == 0x44 && fileBytes[3] == 0x46) return ContentTypeExtension.Mappings["pdf"]; // PDF
        if (fileBytes[0] == 0x3C && fileBytes[1] == 0x3F && fileBytes[2] == 0x78 && fileBytes[3] == 0x6D) return ContentTypeExtension.Mappings["xml"]; // XML
        if (fileBytes[0] == 0x4D && fileBytes[1] == 0x5A) throw new Exception("Dengerous file!!!");// EXE (Windows Executable)
        if (fileBytes.Length > 8 && fileBytes[0] == 0xD0 && fileBytes[1] == 0xCF && fileBytes[2] == 0x11 && fileBytes[3] == 0xE0) return ContentTypeExtension.Mappings["xls"]; // XLS (Older Excel format)
        if (fileBytes.Length > 4 && fileBytes[0] == 0x50 && fileBytes[1] == 0x4B && fileBytes[2] == 0x03 && fileBytes[3] == 0x04) return ContentTypeExtension.Mappings["xlsx"]; // XLSX (Newer Excel format)

        // Check if it's a .bat (Batch file)
        if (IsBatchFile(fileBytes)) throw new Exception("Dengerous file!!!");

        // TXT files don't have a fixed signature, assume if it contains readable ASCII text
        if (IsTextFile(fileBytes)) return ContentTypeExtension.Mappings["txt"];

        return "application/octet-stream"; // Default unknown type
    }
    public async Task<byte[]> Download(string filePath)
    {
        byte[]? fileContent = null;
        if (FileExist(filePath))
        {
            fileContent = await GetBytes(filePath);
        }
        return fileContent;
    }
    public void DeleteFile(string filePath)
    {
        if (FileExist(filePath))
        {
            File.Delete(filePath);
        }
    }
    public byte[] GetBytesFromBase64(string base64)
    {
        return Convert.FromBase64String(base64);
    }
    #region Private Methods
    private bool FileExist(string filePath)
    {
        bool result = false;
        try
        {
            result = File.Exists(filePath);
        }
        catch (Exception)
        {
            result = false;
        }
        return result;

    }
    private string GetCurrentPersianTimestamp()
    {
        var dateTime = DateTime.Now;
        PersianCalendar pc = new PersianCalendar();
        string PersianTimeStamp = $"{pc.GetYear(dateTime)}-{pc.GetMonth(dateTime)}-{pc.GetDayOfMonth(dateTime)} " +
            $"{pc.GetHour(dateTime)}-{pc.GetMinute(dateTime)}-{pc.GetSecond(dateTime)}-{pc.GetMilliseconds(dateTime)}";
        return PersianTimeStamp;
    }
    private async Task<byte[]> GetBytes(string filePath)
    {
        return await File.ReadAllBytesAsync(filePath);
    }

    private bool IsTextFile(byte[] fileBytes)
    {
        foreach (byte b in fileBytes)
        {
            // Check for non-printable characters (except common control characters like tab, new line)
            if (b < 32 && b != 9 && b != 10 && b != 13) return false;
        }
        return true;
    }
    private bool IsBatchFile(byte[] fileBytes)
    {
        string content = Encoding.ASCII.GetString(fileBytes).ToLower();
        return content.Contains("@echo off") || content.Contains("rem ") || content.Contains("::");
    }
    #endregion
}
