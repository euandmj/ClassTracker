using System;
using System.Diagnostics;
using System.Reflection;
using ClassTracker.Exceptions;

namespace ClassTracker
{
    [DebuggerDisplay("{Value}", Name="{Name}")]
    public class TrackingItem<T>
    {
        protected BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public;
        protected MemberInfo Info { get; }
        public string Name { get => Info.Name; }
        public object Value { get; }

        public TrackingItem(T src, MemberInfo info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Value = GetValue(src);

            if(info is PropertyInfo pi)
                if(!pi.CanWrite || !pi.CanRead)
                    throw new MemberInfoException(typeof(T),
                    info, "Member cannot be readonly");
            else if(info is FieldInfo fi)
                if(fi.IsInitOnly)
                    throw new MemberInfoException(typeof(T),
                    info, "Member cannot be readonly");
        }

        public TrackingItem(T src, MemberInfo info, BindingFlags flags)
            : this(src, info)
            => (BindingFlags) 
            = (flags);

        public void SetValue(ref T obj, object value)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            try
            {
                switch (Info.MemberType)
                {
                    case MemberTypes.Property:
                        ((PropertyInfo)Info).SetValue(obj, value);
                        break;
                    case MemberTypes.Field:
                        ((FieldInfo)Info).SetValue(obj, value);
                        break;
                    default:
                        throw new InvalidMemberTypeException(typeof(T), Info.MemberType, Name, "Unsupported MemberInfo type");
                }
            }
            catch(TargetException)
            {
                throw new MemberInfoException(typeof(T), Info, $"A member \"{Name}\" in {typeof(T)} was unable to be set via the MemberInfo");
            }
        }

        public object GetValue(T src)
        {
            if (src is null)
                throw new ArgumentNullException(nameof(src));

            try
            {
                return Info.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)Info).GetValue(src),
                    MemberTypes.Field    => ((FieldInfo)Info).GetValue(src),
                    _                    => throw new InvalidMemberTypeException(typeof(T), Info.MemberType, Name, "Unsupported MemberInfo type")
                };
            }
            catch(TargetException)
            {
                throw new MemberInfoException(typeof(T), Info, $"A member \"{Name}\" in {typeof(T)} was unable to be Get via the MemberInfo");
            }
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}