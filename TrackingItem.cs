using System;
using System.Reflection;

namespace ClassTracker
{
    public class PropertyItem
    {
        public bool HasChanged { get; set; }
        public readonly object Value;
 
        public PropertyItem(object value)
        {
            Value = value;
        }
    }
}