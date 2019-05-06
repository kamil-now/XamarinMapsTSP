using System.ComponentModel;
using System.Reflection;

namespace XamarinTSP.Common.Extensions
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
