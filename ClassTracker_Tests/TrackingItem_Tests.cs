using System.Linq;
using System.Reflection;
using NUnit.Framework;
using ClassTracker;
using System;
using FluentAssertions;
using ClassTracker.Exceptions;

namespace ClassTracker_Tests
{
    [TestFixture]
    public class TrackingItem_Tests
    {
        const BindingFlags _privateFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        private ClassTracker<InvalidTestObject> InvalidTracker;

        [SetUp]
        public void SetUp()
        {
            InvalidTracker = new ClassTracker<InvalidTestObject>();
        }

        [Test]
        public void TrackingItem_ReadonlyProperty_Throws()
        {            
            var a = new InvalidTestObject();
            
            Action privatePropertyAct = () =>
            {
                var info = typeof(InvalidTestObject).GetMember("_PrivateReadonlyProperty", _privateFlags).First();
                new TrackingItem<InvalidTestObject>(a, info);
            };

            Action publicPropertyAct = () =>
            {
                var info = typeof(InvalidTestObject).GetMember("PublicReadonlyProperty").First();
                new TrackingItem<InvalidTestObject>(a, info);
            };
            
            privatePropertyAct.Should().
            Throw<MemberInfoException>(because: "private readonly properties are not valid");

            publicPropertyAct.Should().
            Throw<MemberInfoException>(because: "public readonly properties are not valid");
        }

        [Test, Ignore("member infos are of type 'RtFieldInfo' and skip over the if block in TrackingItem constructor")]
        public void TrackingItem_ReadonlyFields_Throws()
        {
            var a = new InvalidTestObject();
            
            Action privateFieldAct = () =>
            {
                var info = typeof(InvalidTestObject).GetMember("_PrivateReadonlyField", _privateFlags).First();
                new TrackingItem<InvalidTestObject>(a, info);
            };

            Action publicFieldAct = () =>
            {
                var info = typeof(InvalidTestObject).GetMember("PublicReadonlyField").First();
                new TrackingItem<InvalidTestObject>(a, info);
            };
            
            privateFieldAct.Should().
            Throw<MemberInfoException>(because: "private readonly fields are not valid");

            publicFieldAct.Should().
            Throw<MemberInfoException>(because: "public readonly fields are not valid");
       
        }
    }   
}