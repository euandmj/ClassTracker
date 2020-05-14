using System;
using System.Collections.Generic;

namespace ClassTracker
{
    public class PropertyList<T>
    {
        private readonly Dictionary<string, PropertyItem> _properties;

        public PropertyList()
        {
            _properties = new Dictionary<string, PropertyItem>();
        }

        public void AddProperty(string name, object value)
        {
            if (typeof(T).GetProperty(name) is null)
                throw new ArgumentException($"associated type {typeof(T)} does not contain a property named {name}", nameof(name));

            _properties[name] = new PropertyItem(value);
        }

        /// <summary>
        /// Based upon whats changed in A, copy A's changed values to B
        /// </summary>
        /// <param name="a">object to copy from</param>
        /// <param name="b">object to copy to</param>
        /// <returns></returns>
        public T AddTo(T a, T b)
        {
            foreach (var (name, value) in CheckChanged(a))
            {
                typeof(T).GetProperty(name).SetValue(b, value);
            }
            return b;
        }

        /// <summary>
        /// Compare the stored values for the object
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>An enumerable of the properties that have changed and their values</returns>
        public IEnumerable<(string name, object value)> CheckChanged(T obj)
        {
            List<(string, object)> l = new List<(string, object)>();

            foreach (var kvp in _properties)
            {
                var propValue = obj.GetType().GetProperty(kvp.Key).GetValue(obj);

                if (!object.Equals(propValue, kvp.Value.Value))
                {
                    yield return (kvp.Key, propValue);
                }
            }
        }
    }
}