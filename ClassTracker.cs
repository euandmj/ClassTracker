using System;
using System.Collections.Generic;
using System.Reflection;

namespace ClassTracker
{
    public class ClassTracker<T>
    {
        private readonly Dictionary<string, TrackingItem> _properties;

        public ClassTracker()
        {
            _properties = new Dictionary<string, TrackingItem>();
        }

        private void AddItem(string name, TrackingItem item)
        {
            if (_properties.ContainsKey(name))
                throw new ArgumentException("multiple items of the same name are not supported.", nameof(name));

            _properties[name] = item;
        }

        private object GetValue(T src, string name, TrackingItem item)
        {
            var value = item.Info.MemberType switch
            {
                MemberTypes.Property => typeof(T).GetProperty(name).GetValue(src),
                MemberTypes.Field    => typeof(T).GetField(name).GetValue(src),
                _                    => throw new NotSupportedException($"{nameof(MemberTypes)} {item.Info.MemberType} is not supported")
            };
            return value;
        }

        private void SetValue(T obj, string name, object value)
        {
            switch(_properties[name].Info.MemberType)
            {
                case MemberTypes.Property:
                    typeof(T).GetProperty(name).SetValue(obj, value);
                    break;
                case MemberTypes.Field:
                    typeof(T).GetField(name).SetValue(obj, value);
                    break;
            }
        }

        /// <summary>
        /// Records the value of a property
        /// </summary>
        /// <param name="name">The name of the objects property</param>
        /// <param name="value">The value of this property</param>
        public void AddProperty(string name, object value)
        {
            // validate input
            if (!(typeof(T).GetProperty(name) is PropertyInfo info))
                throw new ArgumentException($"associated type {typeof(T)} does not contain a public property named {name}", nameof(name));
            if(info.PropertyType != value.GetType())
                throw new ArgumentException($"types do not match.");
            AddItem(name, new TrackingItem(value, info));
        }

        /// <summary>
        /// Records the value of a field
        /// </summary>
        /// <param name="name">The name of the objects field</param>
        /// <param name="value">The value of this field</param>
        public void AddField(string name, object value)
        {
            //validate input
            if (!(typeof(T).GetField(name) is FieldInfo info))
                throw new ArgumentException($"associated type {typeof(T)} does not contain a public field named {name}", nameof(name));
            if(info.FieldType != value.GetType())
                throw new ArgumentException($"types do not match.");
            AddItem(name, new TrackingItem(value, info));
        }

        // public void AddPrivateField(string name, object value)
        // {
        //     if (!(typeof(T).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic) is FieldInfo info))
        //         throw new ArgumentException($"associated type {typeof(T)} does not contain a private field named {name}", nameof(name));
        //     if(info.FieldType != value.GetType())
        //         throw new ArgumentException("types do not match.");
        //     AddItem(name, new TrackingItem(value, info));
        // }

        /// <summary>
        /// Based upon whats changed in A, copy A's changed values to B
        /// </summary>
        /// <param name="a">object to copy from</param>
        /// <param name="b">object to copy to</param>
        /// <returns>A modified version of B</returns>
        public T AddTo(T a, T b)
        {
            // validate input
            foreach (var (name, value) in CheckChanged(a))
            {
                SetValue(b, name, value);
            }
            // should this even return? we modify the ref b
            return b;
        }

        /// <summary>
        /// Compare the stored values for the object
        /// </summary>
        /// <param name="obj">Object to check against</param>
        /// <returns>An enumerable of the properties that have changed and their values</returns>
        public IEnumerable<(string name, object value)> CheckChanged(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            // T does not need to be validated if it contains the members since they are validated upon add.
            foreach (var kvp in _properties)
            {
                var objValue = GetValue(obj, kvp.Key, kvp.Value);

                if (!object.Equals(objValue, kvp.Value.Value))
                {
                    yield return (kvp.Key, objValue);
                }
            }
        }
    }
}