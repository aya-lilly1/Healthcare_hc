using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HealthCare_Common.Extinsions
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum field) // where TEnum : Enum // C# 7.3
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new Exception($"{nameof(field)} must be enum.");
            }

            var fieldInfo = typeof(TEnum).GetField(field.ToString());

            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).ToArray();

            if (attributes.Length > 0)
            {
                return ((DescriptionAttribute)attributes[0]).Description;
            }

            return "";
        }

        public static TEnum[] GetValues<TEnum>(this TEnum @enum) // where TEnum : Enum
        {
            return (TEnum[])Enum.GetValues(typeof(TEnum));
        }
    }
}
