using System.Reflection;

namespace Tutorial.Commons.Helpers.PropertyFlatten
{
    public record PropertyFlatten(string Path, object? Value, PropertyInfo PropertyInfo, int Depth, int? Index);
}