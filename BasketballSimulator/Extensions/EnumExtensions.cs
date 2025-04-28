using System.ComponentModel;
using System.Reflection;

namespace BasketballSimulator.Core.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Reads the <see cref="DescriptionAttribute"/> on an enum value, if present,
    /// otherwise returns the enum’s name.
    /// </summary>
    public static string GetDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        if (fi != null)
        {
            var attr = fi.GetCustomAttribute<DescriptionAttribute>();
            if (attr != null)
            {
                return attr.Description;
            }
        }
        return value.ToString();
    }
}