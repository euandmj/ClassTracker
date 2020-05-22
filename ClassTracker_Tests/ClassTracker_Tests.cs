using System.Linq;
using NUnit.Framework;
using ClassTracker;

namespace ClassTracker_Tests
{
    [TestFixture]
    public class ClassTracker_Tests
    {
        private ClassTracker<ValidTestObject> Tracker;

        [SetUp]
        public void Setup()
        {
            Tracker = new ClassTracker<ValidTestObject>();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ClassTracker_Register()
        {
            var obj = new ValidTestObject();

            Tracker.Register(obj);

            Assert.AreEqual(Tracker.TrackedCount, obj.NumTrackedItems);

            Tracker.ResetTracker();
        }

        [Test]
        public void ClassTracker_ChangedPublic()
        {
            var obj = new ValidTestObject();

            Tracker.Register(obj);

            obj.PublicField = 10;
            obj.PublicProperty = 10;

            var changed = Tracker.CheckChanged(obj);

            Assert.IsTrue(changed.Any(x => x.name == "PublicField"),    message:"Public field failed tracking");
            Assert.IsTrue(changed.Any(x => x.name == "PublicProperty"), message:"Public property failed tracking");

            Tracker.ResetTracker();
        }

        [Test]
        public void ClassTracker_ChangedPrivate()
        {
            var obj = new ValidTestObject();

            Tracker.Register(obj);

            obj.SetPrivate(10);

            var changed = Tracker.CheckChanged(obj);

            Assert.IsTrue(changed.Any(x => x.name == "_PrivateField"),    message:"Private field failed tracking");
            Assert.IsTrue(changed.Any(x => x.name == "_PrivateProperty"), message:"Private property failed tracking");
            
            Tracker.ResetTracker();
        }

        [Test]
        public void ClassTracker_AddTo()
        {
            var A = new ValidTestObject()
            {
                PublicField = 100
            };
            var B = new ValidTestObject();

            Tracker.Register(A);
            
            A.SetPrivate(20);
            A.PublicProperty = 20;

            Tracker.AssignTo(A, B);

            Assert.AreEqual(A.PublicProperty, B.PublicProperty);
            Assert.AreNotEqual(A.PublicField, B.PublicField);

            Tracker.ResetTracker();
        }

        [Test]
        public void ClassTracker_ResetTracker()
        {
            var a = new ValidTestObject();
            
            Tracker.Register(a);

            int count_after_reg = Tracker.TrackedCount;

            Assert.IsTrue(count_after_reg > 0, message: "there should be tracked items");

            Tracker.ResetTracker();

            int count_after_reset = Tracker.TrackedCount;

            Assert.IsTrue(count_after_reset == 0, message: "the tracker should not no items after reset");
        }

        [Test]
        public void ClassTracker_ResetDefaults()
        {
            const int @default = 10;
            
            var a = new ValidTestObject()
            {
                PublicField = @default,
                PublicProperty = @default
            };

            Tracker.Register(a);

            // change object
            a.PublicField = @default * 2;
            a.PublicProperty = @default * 2;

            Tracker.ResetDefaults(a);

            Assert.IsTrue(a.PublicField == @default,    message: "Public field did not reset");
            Assert.IsTrue(a.PublicProperty == @default, message: "Public property did not reset");

            Tracker.ResetTracker();
        }

        [Test]
        public void ClassTracker_BlindAssignTo()
        {
            var a = new ValidTestObject()
            {
                PublicField = 5,
                PublicProperty = 5
            };
            a.SetPrivate(5);

            var b = new ValidTestObject()
            {
                PublicField = 3,
                PublicProperty = 3
            };
            b.SetPrivate(3);

            Tracker.Register(a);

            // set A into B blindly
            // even though not changed, A state should be set to B
            Tracker.BlindAssignTo(a, b);

            Assert.AreEqual(a.PublicField, b.PublicField);
            Assert.AreEqual(a.PublicProperty, b.PublicProperty);

            Tracker.ResetTracker();
        }
    }
}