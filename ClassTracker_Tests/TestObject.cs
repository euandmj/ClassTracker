using System.Linq;
using System.Reflection;
using ClassTracker;
using ClassTracker.Extensions;

namespace ClassTracker_Tests
{
    public abstract class TestObject
    {
        public virtual int NumTrackedItems
        {
            get
            {
                var publicItems = this.GetType().
                    GetMembers().
                    Where(x => x.HasAttribute<TrackedItemAttribute>()).
                    Count();

                var privateItems = this.GetType().
                    GetMembers(BindingFlags.Instance | BindingFlags.NonPublic).
                    Where(x => x.HasAttribute<TrackedItemAttribute>()).
                    Count();

                return publicItems + privateItems;
            }
        }
    }
    public class ValidTestObject
        : TestObject
    {
        // private
        [TrackedItem] private int _PrivateField;
        [TrackedItem] private int _PrivateProperty { get; set; }

        //public
        [TrackedItem] public int PublicField;
        [TrackedItem] public int PublicProperty { get; set; }

        public int NonTracked;

        public void SetPrivate(int val)
        {
            _PrivateField = val;
            _PrivateProperty = val;
        }
    }

    public class InvalidTestObject
        : TestObject
    {
        [TrackedItem] private readonly int _PrivateReadonlyField;
        [TrackedItem] private int _PrivateReadonlyProperty { get; }

        [TrackedItem] public readonly int PublicReadonlyField;
        [TrackedItem] public int PublicReadonlyProperty { get; }
    }
}