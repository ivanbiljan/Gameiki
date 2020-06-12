using System;
using System.Linq;
using System.Reflection;
using Gameiki.Framework.Commands;

namespace Gameiki.Extensions {
    public static class ParameterInfoExtensions {
        public static T GetCustomAttribute<T>(this ParameterInfo memberInfo, bool inherit = false) where T : Attribute {
            return memberInfo.GetCustomAttributes(typeof(T), false).ElementAtOrDefault(0) as T;
        }
        
        public static bool IsOptional(this ParameterInfo parameterInfo) {
            return parameterInfo.IsOptional || parameterInfo.GetCustomAttribute<ParameterAttribute>().IsOptional ||
                   parameterInfo.GetCustomAttributes(typeof(DefaultAttribute), false).Length > 0 ||
                   parameterInfo.GetType().IsGenericTypeDefinition &&
                   parameterInfo.GetType().GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}