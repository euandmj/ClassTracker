using System.Linq;
using System.Reflection;
using ClassTracker;

namespace ClassTracker_Tests
{
    public class TestObject
    {
        // private
        [TrackedItem]private int _PrivateField;
        [TrackedItem]private int _PrivateProperty { get; set; }

        //public
        [TrackedItem]public int PublicField;
        [TrackedItem]public int PublicProperty { get; set; }

        public int NonTracked;

        public TestObject(){}

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