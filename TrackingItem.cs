using System;
using System.Reflection;

namespace ClassTracker
{
    public class TrackingItem
    {
        public readonly MemberTypes ItemType;
        public readonly object Value;
    
 
        public TrackingItem(object value, MemberTypes itemType)
        {
            Value = value;
            ItemType = itemType;
        }
    }
}