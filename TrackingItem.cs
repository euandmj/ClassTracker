using System;
using System.Reflection;

namespace ClassTracker
{
    public class TrackingItem
    {
        public readonly MemberInfo Info;
        public readonly object Value;
    
 
        public TrackingItem(object value, MemberInfo info)
        {
            Value = value;
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }
    }
}