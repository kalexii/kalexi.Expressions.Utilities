using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using NUnit.Framework;

namespace kalexi.Expressions.Utilities.Tests
{
    [TestFixture]
    public class ExpressionUtilitiesTests
    {
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private class Dummy
        {
            public string StringProperty { get; set; }
            public int IntProperty { get; set; }

#pragma warning disable 649
            public string StringField;
            public int IntField;
#pragma warning restore 649

            public void VoidMethod()
            {
            }
            public string StringMethod() => StringProperty;
            public int IntMethod() => IntProperty;
        }

        private static Expression<Func<T, object>> FuncObjectGate<T>(Expression<Func<T, object>> expression) => expression;
        private static Expression<Func<T, TR>> FuncGate<T,TR>(Expression<Func<T, TR>> expression) => expression;
        private static Expression<Action<T>> ActionGate<T>(Expression<Action<T>> expression) => expression;

        [Test]
        public void GetsReferenceTypeProperty()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.StringProperty).GetProperty());

        [Test]
        public void GetsValueTypeProperty()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.IntProperty).GetProperty());

        [Test]
        public void GetsReferenceTypeField()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.StringField).GetField());

        [Test]
        public void GetsValueTypeField()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.IntField).GetField());

        [Test]
        public void GetsMethodInfoForVoidMethod()
            => Assert.DoesNotThrow(() => ActionGate<Dummy>(x => x.VoidMethod()).GetMethodInfo());

        [Test]
        public void GetsMethodInfoForStringMethod()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.StringMethod()).GetMethodInfo());

        [Test]
        public void GetsMethodInfoForIntMethod()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.IntMethod()).GetMethodInfo());

        [Test]
        public void CreatesGetterForValueType()
        {
            var getter = FuncGate<Dummy, int>(x => x.IntProperty).CreateGetter();
            var dummy = new Dummy {IntProperty = 123};
            Assert.AreEqual(getter(dummy), 123);
        }

        [Test]
        public void CreatesGetterForReferenceType()
        {
            var getter = FuncGate<Dummy, string>(x => x.StringProperty).CreateGetter();
            var dummy = new Dummy {StringProperty = "string"};
            Assert.AreEqual(getter(dummy), "string");
        }

        [Test]
        public void CreatesSetterForValueType()
        {
            var setter = FuncGate<Dummy, int>(x => x.IntProperty).CreateSetter();
            var dummy = new Dummy();
            setter(dummy, 123);
            Assert.AreEqual(dummy.IntProperty, 123);
        }

        [Test]
        public void CreatesSetterForReferenceType()
        {
            var setter = FuncGate<Dummy, string>(x => x.StringProperty).CreateSetter();
            var dummy = new Dummy();
            setter(dummy, "string");
            Assert.AreEqual(dummy.StringProperty, "string");
        }
    }
}
