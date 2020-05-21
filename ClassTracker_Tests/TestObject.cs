using System.Linq;
using System.Reflection;
using ClassTracker;

namespace ClassTracker_Tests
{
    public class TestObject
    {
        // private
        [TrackedItem]private int _PrivateField;
        [TrackedItem]private readonly int _PrivateReadOnlyField;
        [TrackedItem]private int _PrivateProperty { get; set; }
        [TrackedItem]private int _PrivateReadOnlyProperty { get; }

        //public
        [TrackedItem]public int PublicField;
        [TrackedItem]public readonly int PublicReadOnlyField;
        [TrackedItem]public int PublicProperty { get; set; }
        [TrackedItem]public int PublicReadOnlyProperty { get; }

        public int NonTracked;

        public TestObject(){}
        public TestObject(int val)
        {
            _PrivateReadOnlyField = val;
            _PrivateReadOnlyProperty = val;
            PublicReadOnlyField = val;
            PublicReadOnlyProperty = val;
        }

        public int NumTrackedItems
        {
            get
            {
                var publicItems = typeof(TestObject).
                    GetMembers().
                    Where(x => x.GetCustomAttributes(typeof(TrackedItemAttribute)).Any()).
                    Count();

                var privateItems = typeof(TestObject).
                    GetMembers(BindingFlags.Instance | BindingFlags.NonPublic).
                    Where(x => x.GetCustomAttributes(typeof(TrackedItemAttribute)).Any()).
                    Count();

                return publicItems + privateItems;
            }
        }

        public void SetPrivate(int val)
        {
            _PrivateField = val;
            _PrivateProperty = val;
        }
    }
}