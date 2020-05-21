using System.Linq;
using NUnit.Framework;
using FluentAssertions.Common;
using ClassTracker;
namespace ClassTracker_Tests
{
    [TestFixture]
    public class ClassTracker_Tests
    {
        private ClassTracker<TestObject> Tracker;

        [SetUp]
        public void Setup()
        {
            Tracker = new ClassTracker<TestObject>();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void ClassTracker_Register()
        {
            var obj = new TestObject();

            Tracker.Register(obj);

            Assert.AreEqual(Tracker.TrackedCount, obj.NumTrackedItems);

            Tracker.Reset();
        }

        [Test]
        public void ClassTracker_ChangedPublic()
        {
            var obj = new TestObject();

            Tracker.Register(obj);

            obj.PublicField = 10;
            obj.PublicProperty = 10;

            var changed = Tracker.CheckChanged(obj);

            Assert.IsTrue(changed.Any(x => x.name == "PublicField"),    message:"Public field failed tracking");
            Assert.IsTrue(changed.Any(x => x.name == "PublicProperty"), message:"Public property failed tracking");

            Tracker.Reset();
        }

        [Test]
        public void ClassTracker_ChangedPrivate()
        {
            var obj = new TestObject();

            Tracker.Register(obj);

            obj.SetPrivate(10);

            var changed = Tracker.CheckChanged(obj);

            Assert.IsTrue(changed.Any(x => x.name == "_PrivateField"),    message:"Private field failed tracking");
            Assert.IsTrue(changed.Any(x => x.name == "_PrivateProperty"), message:"Private property failed tracking");
            
            Tracker.Reset();
        }

        [Test]
        public void ClassTracker_AddTo()
        {
            var A = new TestObject()
            {
                PublicField = 100
            };
            var B = new TestObject();

            Tracker.Register(A);
            
            A.SetPrivate(20);
            A.PublicProperty = 20;

            Tracker.AssignTo(A, B);

            Assert.AreEqual(A.PublicProperty, B.PublicProperty);
            Assert.AreNotEqual(A.PublicField, B.PublicField);

            Tracker.Reset();
        }
    }
}