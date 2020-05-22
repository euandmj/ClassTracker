using System;
using System.Reflection;

namespace ClassTracker.Extensions
{
    public static class MemberInfoExtensions
    {
        public static bool HasAttribute<T>(this MemberInfo mem) where T : Attribute
        {
            var attrs = mem.GetCustomAttribute(typeof(T));
            return !(attrs is null);
        }
    }
}