using System;
using System.Reflection;

namespace ClassTracker
{
    public class NonAttributedClassTracker<T>
        : ClassTracker<T>
    {


        public NonAttributedClassTracker()
            : base()
        {
            
        }

        /// <summary>
        /// Records the value of a property
        /// </summary>
        /// <param name="name">The name of the objects property</param>
        /// <param name="obj">The object to get the property from</param>
        public void RegisterProperty(string propertyName, T obj)
        {
            if(typeof(T).GetProperty(propertyName) is PropertyInfo info)
                AddItem(propertyName, new TrackingItem<T>(obj, info));
            else if(typeof(T).GetProperty(propertyName, _privateFlags) is PropertyInfo privateInfo)
                AddItem(propertyName, new TrackingItem<T>(obj, privateInfo));
            else
                throw new ArgumentException($"associated type {typeof(T)} does not contain a property named {propertyName}", nameof(propertyName));
        }

        /// <summary>
        /// Records the value of a field
        /// </summary>
        /// <param name="name">The name of the objects field</param>
        /// <param name="obj">The object to get the field from</param>
        public void RegisterField(string fieldName, T obj)
        {
            if(typeof(T).GetField(fieldName) is FieldInfo info)
                AddItem(fieldName, new TrackingItem<T>(obj, info));
            else if(typeof(T).GetField(fieldName, _privateFlags) is FieldInfo privateInfo)
                AddItem(fieldName, new TrackingItem<T>(obj, privateInfo));
            else
                throw new ArgumentException($"associated type {typeof(T)} does not contain a field named {fieldName}", nameof(fieldName));
        }
    }
}