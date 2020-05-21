using System;

namespace ClassTracker.Exceptions
{
    public class MemberInfoException
        : Exception
    {
        private Type AssociatedType { get; }
        private System.Reflection.MemberInfo MemberInfo { get; }
        private System.Reflection.MemberTypes MemberType { get => this.MemberInfo.MemberType; }

        public MemberInfoException(
            Type @type,
            System.Reflection.MemberInfo memberInfo,
            string message)
            : base(message)
        {
            AssociatedType = type ?? throw new ArgumentNullException(nameof(@type));
            MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
        }
    }
}
