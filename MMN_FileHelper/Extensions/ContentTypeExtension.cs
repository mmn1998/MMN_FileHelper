namespace MMN_FileHelper.Extensions;

public static class ContentTypeExtension
{
    public static readonly Dictionary<string, string> Mappings = new Dictionary<string, string>()
    {
        {"txt", "text/plain" },
        {"xls", "application/vnd.ms-excel" },
        {"xlsx", "application/vnd.ms-excel" },
        {"csv", "text/cs" },
        {"pdf", "application/pdf" },
        {"png", "image/png" },
        {"jpg", "image/jpeg" },
        {"jpeg", "image/jpeg" },
        {"xml", "application/xml" },
        {"exe", "application/x-msdownload"},
        {"bat", "application/x-bat"}
    };
    public static string GetContentType(this string extension)
    {
        return Mappings[extension];
    }
}