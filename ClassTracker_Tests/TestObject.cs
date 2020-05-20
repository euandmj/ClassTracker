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

    }
}