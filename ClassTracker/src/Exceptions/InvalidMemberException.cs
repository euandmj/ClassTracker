using System;

namespace ClassTracker.Exceptions
{
    public class InvalidMemberTypeException
        : Exception
    {
        public Type AssociatedType { get; }
        public System.Reflection.MemberTypes MemberType { get; }
        public string Name { get; }

        public InvalidMemberTypeException(
            Type @type,
            System.Reflection.MemberTypes memberType,
            string name,
            string message)
            : base(message) 
        {
            AssociatedType = @type ?? throw new ArgumentNullException(nameof(@type));
            MemberType = memberType;
            Name = name ?? throw new ArgumentNullException(nameof(name));            
        }
    }
}