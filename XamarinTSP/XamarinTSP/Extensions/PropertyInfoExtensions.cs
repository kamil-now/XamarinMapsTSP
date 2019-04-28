using System.ComponentModel;
using System.Reflection;

namespace XamarinTSP.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string GetDescription(this PropertyInfo property)
        {
            return property
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }
    }
}
