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
            var value = item.ItemType switch
            {
                MemberTypes.Property => typeof(T).GetProperty(name).GetValue(src),
                MemberTypes.Field    => typeof(T).GetField(name).GetValue(src),
                _                    => throw new NotSupportedException($"{nameof(MemberTypes)} {item.ItemType} is not supported")
            };
            return value;
        }

        private void SetValue(T obj, string name, object value)
        {
            switch(_properties[name].ItemType)
            {
                case MemberTypes.Property:
                    typeof(T).GetProperty(name).SetValue(obj, value);
                    break;
                case MemberTypes.Field:
                    typeof(T).GetField(name).SetValue(obj, value);
                    break;
            }
        }

        public void AddProperty(string name, object value)
        {
            // validate input
            if (typeof(T).GetProperty(name) is null)
                throw new ArgumentException($"associated type {typeof(T)} does not contain a property named {name}", nameof(name));

            AddItem(name, new TrackingItem(value, MemberTypes.Property));
        }

        public void AddField(string name, object value)
        {
            //validate input
            if (typeof(T).GetField(name) is null)
                throw new ArgumentException($"associated type {typeof(T)} does not contain a field named {name}", nameof(name));

            AddItem(name, new TrackingItem(value, MemberTypes.Field));
        }

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