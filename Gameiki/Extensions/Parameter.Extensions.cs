using System;
using System.Reflection;
using Gameiki.Framework.Commands;
using ReLogic.Utilities;

namespace Gameiki.Extensions {
    public static class ParameterExtensions {
        public static bool IsOptional(this ParameterInfo parameterInfo) {
            return parameterInfo.IsOptional /*|| parameterInfo.GetCustomAttributes(typeof(OptionalAttribute), false).Length > 0*/ ||
                   parameterInfo.GetCustomAttributes(typeof(DefaultAttribute), false).Length > 0 ||
                   parameterInfo.GetType().IsGenericTypeDefinition &&
                   parameterInfo.GetType().GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}