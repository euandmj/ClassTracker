using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ClassTracker
{
    public class ClassTracker<T>
    {
        private readonly HashSet<TrackingItem<T>> _properties;

        public ClassTracker()
        {
            _properties = new HashSet<TrackingItem<T>>();
        }

        private void AddItem(string name, TrackingItem<T> item)
        {
            if (_properties.Any(x => x.Name == name))
                throw new ArgumentException("multiple items of the same name are not supported. Name: " + name, nameof(name));
            
            _properties.Add(item);
        }

        private IEnumerable<(object newValue, TrackingItem<T>)> GetChanged(T obj)
        {
            foreach (var item in _properties)
            {
                object objVal = item.GetValue(obj);

                if (!Equals(objVal, item.Value))
                    yield return (objVal, item);
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
            if (info.PropertyType != value.GetType())
                throw new ArgumentException($"types do not match.");
            AddItem(name, new TrackingItem<T>(value, info));
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
            if (info.FieldType != value.GetType())
                throw new ArgumentException($"types do not match.");
            AddItem(name, new TrackingItem<T>(value, info));
        }

        /// <summary>
        /// Registers all public members of the object that have a <see cref="TrackedItemAttribute"/>
        /// </summary>
        public void Register(T obj)
        {
            var members = typeof(T).GetMembers();

            foreach(var mem in members)
            {               
                var attribute = mem.GetCustomAttribute(typeof(TrackedItemAttribute));

                if(attribute is null) continue;
                AddItem(mem.Name, new TrackingItem<T>(obj, mem));                
            }
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
        public void AddTo(T a, T b)
        {
            // validate input
            foreach (var (newVal, item) in GetChanged(a))
            {
                item.SetValue(ref b, newVal);
            }
        }

        /// <summary>
        /// Compare the stored values for the object
        /// </summary>
        /// <param name="obj">Object to check against</param>
        /// <returns>An enumerable of the properties that have changed and their initial values</returns>
        public IEnumerable<(string name, object value)> CheckChanged(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            var changed = GetChanged(obj);

            return changed.Select(x => (x.Item2.Name, x.Item2.Value));
        }
    }
}