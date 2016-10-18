using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public static class BaseExtensions
{
    public static string ToDescription(this Enum value)
    {
        try
        {
            if (value == null) { return string.Empty; }

            var attributes = (DescriptionAttribute[])value.GetType().GetField(Convert.ToString(value)).GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : Convert.ToString(value);
        }
        catch
        {
            return value.ToString();
        }
    }

    public static string DisplayName(this Enum value)
    {
        Type enumType = value.GetType();
        var enumValue = Enum.GetName(enumType, value);
        MemberInfo member = enumType.GetMember(enumValue)[0];

        var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
        var outString = ((DisplayAttribute)attrs[0]).Name;

        if (((DisplayAttribute)attrs[0]).ResourceType != null)
        {
            outString = ((DisplayAttribute)attrs[0]).GetName();
        }

        return outString;
    } 
}

