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
        // if i can figure out BindingFlags.Default then 
        // here would be a good place to hold them
        public BindingFlags BindingFlags { get; }

        public TrackedItemAttribute(BindingFlags binding = BindingFlags.Default)
        {   
            // this.BindingFlags = binding;
        }
    }
}