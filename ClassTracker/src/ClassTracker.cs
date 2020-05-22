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

                if (!Equals(objVal, item.RecordedValue))
                    yield return (objVal, item);
            }
        }

        /// <summary>
        /// Registers all public and private members of the object that have a <see cref="TrackedItemAttribute"/>
        /// </summary>
        /// <exception cref="MemberInfoException">If the object has a non-mutatable attributed member</exception>
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

        public void ResetTracker()
        {
            _properties.Clear();
        }

        /// <summary>
        /// Resets an instance of T to its recorded values
        /// </summary>
        public T ResetDefaults(T obj)
        {
            foreach(var item in _properties)
            {
                item.SetValue(ref obj, item.RecordedValue);
            }
            return obj;
        }

        /// <summary>
        /// Based upon whats changed in A, copy A's changed values to B
        /// </summary>
        /// <param name="a">object to copy from</param>
        /// <param name="b">object to copy to</param>
        public void AssignTo(T a, T b)
        {
            // validate input
            foreach (var (newVal, item) in GetChanged(a))
            {
                item.SetValue(ref b, newVal);
            }
        }

        /// <summary>
        /// Applies all tracked properties from A in their current state, to B
        /// </summary>
        /// <param name="a">object to copy from</param>
        /// <param name="b">object to copy to</param>
        public void BlindAssignTo(T a, T b)
        {
            foreach(var item in _properties)
            {
                item.SetValue(ref b, item.GetValue(a));
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

            return changed.Select(x => (x.Item2.Name, x.Item2.RecordedValue));
        }
    }
}