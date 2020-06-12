using System;
using System.Linq;
using System.Reflection;

namespace Gameiki.Extensions {
    public static class MemberInfoExtensions {
        public static T GetCustomAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute {
            return memberInfo.GetCustomAttributes(typeof(T), false).ElementAtOrDefault(0) as T;
        }
    }
}