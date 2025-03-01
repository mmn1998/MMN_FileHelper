using Microsoft.AspNetCore.Http;

namespace SIMA.Framework.Common.Helper.FileHelper;

public interface IFileService
{
    string? GetMimeType(byte[] fileContent);
    Task<string> Upload(byte[] fileContent, string filename, string rootPath);
    Task<string> Upload(IFormFile file, string rootPath);
    Task<byte[]> Download(string filePath);
    byte[] GetBytesFromBase64(string base64);
    void DeleteFile(string filePath);
}