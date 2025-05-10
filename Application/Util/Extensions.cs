using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Util
{
    /// <summary>
    /// Retrieves the description of an enum value using the <see cref="DescriptionAttribute"/> 
    /// </summary>
    /// <param name="value">The enum value to get the description for</param>
    /// <returns>
    /// The text from the <see cref="DescriptionAttribute"/> if present, 
    /// otherwise the enum name as a string
    /// </returns>
    /// <example>
    /// <code>
    /// enum TestEnum {
    ///     [Description("First value")]
    ///     Value1,
    ///     Value2
    /// }
    /// 
    /// TestEnum.Value1.GetDescription(); // returns "First value"
    /// TestEnum.Value2.GetDescription(); // returns "Value2"
    /// </code>
    /// </example>
    /// <remarks>
    /// This method uses reflection to inspect the enum value's attributes.
    /// Consider caching results if performance is critical.
    /// </remarks>
    public static class Extensions
    {
        public static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            string name = Enum.GetName(enumType, value);
            if (name == null) return value.ToString();

            FieldInfo field = enumType.GetField(name);
            DescriptionAttribute? attribute =
                Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                as DescriptionAttribute;

            return attribute?.Description ?? name;
        }
    }
}
