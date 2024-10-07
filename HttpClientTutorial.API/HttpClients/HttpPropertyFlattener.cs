using HttpClientTutorial.API.Helpers.PropertyFlatten;
using System.Reflection;

namespace HttpClientTutorial.API.HttpClients
{
    public class HttpPropertyFlattener : PropertyFlattener
    {
        public HttpPropertyFlattener(PropertyFlattenOptions? options = null)
            : base(options)
        {
        }

        public override string GetName(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<FormNameAttribute>()?.Name ?? base.GetName(propertyInfo);
        }
    }
}