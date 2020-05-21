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

        public int TrackedCount { get => _properties.Count; }

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

                if(item.Name == "PublicReadOnlyField")
                    {
                        _ =5;
                    }

                if (!Equals(objVal, item.Value))
                    yield return (objVal, item);
            }
        }

        /// <summary>
        /// Registers all public and private members of the object that have a <see cref="TrackedItemAttribute"/>
        /// </summary>
        public void Register(T obj)
        {
            if(obj is null)
                throw new ArgumentNullException(nameof(obj));

            const BindingFlags privateFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            // add public members
            foreach (var mem in typeof(T).GetMembers())
            {
                if (mem.GetCustomAttributes(typeof(TrackedItemAttribute)).Any())
                    AddItem(mem.Name, new TrackingItem<T>(obj, mem));
            }

            // add private members
            foreach (var mem in typeof(T).GetMembers(privateFlags))
            {
                if (mem.GetCustomAttributes(typeof(TrackedItemAttribute)).Any())
                    AddItem(mem.Name, new TrackingItem<T>(obj, mem, privateFlags));
            }
        }

        public void Reset()
        {
            _properties.Clear();
        }

        /// <summary>
        /// Based upon whats changed in A, copy A's changed values to B
        /// </summary>
        /// <param name="a">object to copy from</param>
        /// <param name="b">object to copy to</param>
        /// <returns>A modified version of B</returns>
        public void AssignTo(T a, T b)
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