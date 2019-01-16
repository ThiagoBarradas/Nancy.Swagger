using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;

namespace Nancy.Swagger.Helpers
{
    public static class PropertyInfoHelper
    {
        public static string GetNameConsideringNewtonsoft(PropertyInfo propertyInfo, JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                return propertyInfo.Name.ToCamelCase();
            }

            var contract = settings.ContractResolver.ResolveContract(propertyInfo.DeclaringType) as JsonObjectContract;

            var currentProperty = contract?.Properties?.FirstOrDefault(r => r.UnderlyingName == propertyInfo.Name);

            if (currentProperty == null)
            {
                return propertyInfo.Name;
            }

            return currentProperty.PropertyName;
        }s
    }
}
