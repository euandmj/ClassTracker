using System;

namespace ClassTracker
{
    public class InvalidMemberException
        : Exception
    {
        public Type AssociatedType { get; }
        public System.Reflection.BindingFlags BindingFlags { get; }
        public string Name { get; }

        public InvalidMemberException(
            Type @type,
            System.Reflection.BindingFlags flags,
            string name) 
        {
            AssociatedType = @type ?? throw new ArgumentNullException(nameof(@type));
            BindingFlags = flags;
            Name = name ?? throw new ArgumentNullException(nameof(name));            
        }
    }
}