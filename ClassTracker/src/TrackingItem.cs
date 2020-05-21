using System;
using System.Diagnostics;
using System.Reflection;

namespace ClassTracker
{
    [DebuggerDisplay("{Value}", Name="{Name}")]
    public class TrackingItem<T>
    {
        protected BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public;
        protected MemberInfo Info { get; }
        public string Name { get => Info.Name; }
        public object? Value { get; }

        public TrackingItem(T src, MemberInfo info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Value = GetValue(src);
        }

        public TrackingItem(T src, MemberInfo info, BindingFlags flags)
            : this(src, info)
            => (BindingFlags) 
            = (flags);

        public void SetValue(ref T obj, object value)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            switch (Info.MemberType)
            {
                case MemberTypes.Property:
                    ((PropertyInfo)Info).SetValue(obj, value);
                    break;
                case MemberTypes.Field:
                    ((FieldInfo)Info).SetValue(obj, value);
                    break;
                default:
                    throw new InvalidMemberException(typeof(T), BindingFlags, Name);
            }
        }

        public object GetValue(T src)
        {
            if (src is null)
                throw new ArgumentNullException(nameof(src));

            return Info.MemberType switch
            {
                MemberTypes.Property => ((PropertyInfo)Info).GetValue(src),
                MemberTypes.Field    => ((FieldInfo)Info).GetValue(src),
                _                    => throw new InvalidMemberException(typeof(T), BindingFlags, Name)
            };
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}