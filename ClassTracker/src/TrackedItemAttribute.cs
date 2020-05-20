using System;
using System.Reflection;

namespace ClassTracker
{    
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field,
        Inherited=true,
        AllowMultiple=false)]
    public class TrackedItemAttribute
        : Attribute
    {
    }
}