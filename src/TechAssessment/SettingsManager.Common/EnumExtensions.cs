using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SettingsManager.Common
{
    public static class EnumExtensions
    {
        public static string? DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            if (enumValue == null)
            {
                return String.Empty;
            }

            MemberInfo member = enumType.GetMember(enumValue)[0];

            string? outString = value.ToString();

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs.Length > 0 && ((DisplayAttribute)attrs[0]).Name != null)
            {
                outString = ((DisplayAttribute)attrs[0]).Name;

                if (((DisplayAttribute)attrs[0]).ResourceType != null)
                {
                    outString = ((DisplayAttribute)attrs[0]).GetName();
                }
            }

            return outString;
        }
    }
}
