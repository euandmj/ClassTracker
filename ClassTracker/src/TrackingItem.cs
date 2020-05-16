using System;
using System.Reflection;

namespace ClassTracker
{
    public class TrackingItem<T>
    {
        protected MemberInfo Info { get; }
        public string Name { get => Info.Name; }
        public object? Value { get; }

        public TrackingItem(object value, MemberInfo info)
        {
            Value = value;
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }

        public void SetValue(ref T obj, object value)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            switch (Info.MemberType)
            {
                case MemberTypes.Property:
                    typeof(T).GetProperty(Name).SetValue(obj, value);
                    break;
                case MemberTypes.Field:
                    typeof(T).GetField(Name).SetValue(obj, value);
                    break;
            }
        }

        public object GetValue(T src)
        {
            if (src is null)
                throw new ArgumentNullException(nameof(src));

            return Info.MemberType switch
            {
                MemberTypes.Property => typeof(T).GetProperty(Name).GetValue(src),
                MemberTypes.Field    => typeof(T).GetField(Name).GetValue(src),
                _                    => throw new NotSupportedException($"{nameof(MemberTypes)} {Info.MemberType} is not supported")
            };
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}