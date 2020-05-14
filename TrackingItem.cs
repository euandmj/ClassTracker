using System;
using System.Reflection;

namespace ClassTracker
{
    public class TrackingItem
    {
        public MemberInfo Info { get; }
        public object Value { get; }    
 
        public TrackingItem(object value, MemberInfo info)
        {
            Value = value;
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }
    }
}