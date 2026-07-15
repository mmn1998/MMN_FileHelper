using System.Reflection;

namespace MMN_FileHelper.Extensions;

public static class ReflectionExtension
{
    public static (int Count, List<string> Names) GetPropertyInfo<T>()
    {
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => !p.Name.Contains("Id")) // case-sensitive exclusion
            .Select(p => p.Name)
            .ToList();

        return (properties.Count, properties);
    }
}
