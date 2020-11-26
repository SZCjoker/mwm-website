using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Phoenixnet.Extensions.Attribute;

namespace Phoenixnet.Extensions.Method
{
    public static class EnumExtension
    {
        public static string Description(this Enum value)
        {
            return value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }

        public static string Code(this Enum value)
        {
            return value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<CodeAttribute>()
                ?.Code;
        }
    }
}