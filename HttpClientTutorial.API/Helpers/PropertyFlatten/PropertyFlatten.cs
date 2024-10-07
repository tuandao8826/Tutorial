using System.Reflection;

namespace HttpClientTutorial.API.Helpers.PropertyFlatten
{
    public record PropertyFlatten(string Path, object? Value, PropertyInfo PropertyInfo, int Depth, int? Index);
}