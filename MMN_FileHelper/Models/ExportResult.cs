namespace MMN_FileHelper.Models;

public class ExportResult
{
    public ExportResult(byte[] fileContent, string extension, string contentType, string name)
    {
        FileContent = fileContent;
        Extension = extension;
        ContentType = contentType;
        Name = name;
    }
    public byte[] FileContent { get; init; }

    public string Extension { get; init; }

    public string ContentType { get; init; }

    public string Name { get; init; }
}
