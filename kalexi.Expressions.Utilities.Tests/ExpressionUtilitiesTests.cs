using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
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

        #region Retrieving property by expression

        [Test]
        public void GetsReferenceTypeProperty()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.StringProperty).GetProperty());

        [Test]
        public void GetsValueTypeProperty()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.IntProperty).GetProperty());

        #endregion

        #region Retrieving field by expression

        [Test]
        public void GetsReferenceTypeField()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.StringField).GetField());

        [Test]
        public void GetsValueTypeField()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.IntField).GetField());

        #endregion

        #region Retrieving methodInfo by expression

        [Test]
        public void GetsMethodInfoForVoidMethod()
            => Assert.DoesNotThrow(() => ActionGate<Dummy>(x => x.VoidMethod()).GetMethodInfo());

        [Test]
        public void GetsMethodInfoForStringMethod()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.StringMethod()).GetMethodInfo());

        [Test]
        public void GetsMethodInfoForIntMethod()
            => Assert.DoesNotThrow(() => FuncObjectGate<Dummy>(x => x.IntMethod()).GetMethodInfo());

        #endregion

        #region getter by expression

        [Test]
        public void CreatesGetterByExpressionForValueType()
        {
            var getter = FuncGate<Dummy, int>(x => x.IntProperty).CreateGetter();
            var dummy = new Dummy {IntProperty = 123};
            Assert.AreEqual(getter(dummy), 123);
        }

        [Test]
        public void CreatesGetterByExpressionForReferenceType()
        {
            var getter = FuncGate<Dummy, string>(x => x.StringProperty).CreateGetter();
            var dummy = new Dummy {StringProperty = "string"};
            Assert.AreEqual(getter(dummy), "string");
        }

        #endregion 

        #region getter by property

        [Test]
        public void CreatesGetterByPropertyForReferenceType()
        {
            var getter = Property(nameof(Dummy.StringProperty)).CreateGetter<Dummy>();
            var dummy = new Dummy {StringProperty = "string"};
            Assert.AreEqual(getter(dummy), "string");
        }

        [Test]
        public void CreatesGetterByPropertyForValueType()
        {
            var getter = Property(nameof(Dummy.IntProperty)).CreateGetter<Dummy>();
            var dummy = new Dummy {IntProperty = 123};
            Assert.AreEqual(getter(dummy), 123);
        }

        #endregion

        #region getter by field

        [Test]
        public void CreatesGetterByFieldForReferenceType()
        {
            var getter = Field(nameof(Dummy.StringField)).CreateGetter<Dummy>();
            var dummy = new Dummy {StringField = "string"};
            Assert.AreEqual(getter(dummy), "string");
        }

        [Test]
        public void CreatesGetterByFieldForValueType()
        {
            var getter = Field(nameof(Dummy.IntField)).CreateGetter<Dummy>();
            var dummy = new Dummy {IntField = 123};
            Assert.AreEqual(getter(dummy), 123);
        }

        #endregion

        #region untyped getter by property

        [Test]
        public void CreatesUntypedGetterByPropertyForReferenceType()
        {
            var getter = Property(nameof(Dummy.StringProperty)).CreateUntypedGetter();
            var dummy = new Dummy {StringProperty = "string"};
            Assert.AreEqual(getter(dummy), "string");
        }

        [Test]
        public void CreatesUntypedGetterByPropertyForValueType()
        {
            var getter =  Property(nameof(Dummy.IntProperty)).CreateUntypedGetter();
            var dummy = new Dummy {IntProperty = 123};
            Assert.AreEqual(getter(dummy), 123);
        }

        #endregion

        #region untyped getter by field

        [Test]
        public void CreatesUntypedGetterByFieldForReferenceType()
        {
            var getter = Field(nameof(Dummy.StringField)).CreateUntypedGetter();
            var dummy = new Dummy {StringField = "string"};
            Assert.AreEqual(getter(dummy), "string");
        }

        [Test]
        public void CreatesUntypedGetterByFieldForValueType()
        {
            var getter = Field(nameof(Dummy.IntField)).CreateUntypedGetter();
            var dummy = new Dummy {IntField = 123};
            Assert.AreEqual(getter(dummy), 123);
        }

        #endregion

        #region setter by expression

        [Test]
        public void CreatesSetterByExpressionForValueType()
        {
            var setter = FuncGate<Dummy, int>(x => x.IntProperty).CreateSetter();
            var dummy = new Dummy();
            setter(dummy, 123);
            Assert.AreEqual(dummy.IntProperty, 123);
        }

        [Test]
        public void CreatesSetterByExpressionForReferenceType()
        {
            var setter = FuncGate<Dummy, string>(x => x.StringProperty).CreateSetter();
            var dummy = new Dummy();
            setter(dummy, "string");
            Assert.AreEqual(dummy.StringProperty, "string");
        }

        [Test]
        public void CreatesSetterByPropertyForValueType()
        {
            var setter = Property(nameof(Dummy.IntProperty)).CreateSetter<Dummy>();
            var dummy = new Dummy();
            setter(dummy, 123);
            Assert.AreEqual(dummy.IntProperty, 123);
        }

        [Test]
        public void CreatesSetterByPropertyForReferenceType()
        {
            var setter = Property(nameof(Dummy.StringProperty)).CreateSetter<Dummy>();
            var dummy = new Dummy();
            setter(dummy, "string");
            Assert.AreEqual(dummy.StringProperty, "string");
        }

        [Test]
        public void CreatesSetterByFieldForValueType()
        {
            var setter = Field(nameof(Dummy.IntField)).CreateSetter<Dummy>();
            var dummy = new Dummy();
            setter(dummy, 123);
            Assert.AreEqual(dummy.IntField, 123);
        }

        [Test]
        public void CreatesSetterByFieldForReferenceType()
        {
            var setter = Field(nameof(Dummy.StringField)).CreateSetter<Dummy>();
            var dummy = new Dummy();
            setter(dummy, "string");
            Assert.AreEqual(dummy.StringField, "string");
        }

        [Test]
        public void CreatesUntypedSetterByPropertyForValueType()
        {
            var setter = Property(nameof(Dummy.IntProperty)).CreateUntypedSetter();
            var dummy = new Dummy();
            setter(dummy, 123);
            Assert.AreEqual(dummy.IntProperty, 123);
        }

        [Test]
        public void CreatesUntypedSetterByPropertyForReferenceType()
        {
            var setter = Property(nameof(Dummy.StringProperty)).CreateUntypedSetter();
            var dummy = new Dummy();
            setter(dummy, "string");
            Assert.AreEqual(dummy.StringProperty, "string");
        }

        [Test]
        public void CreatesUntypedSetterByFieldForValueType()
        {
            var setter = Field(nameof(Dummy.IntField)).CreateUntypedSetter();
            var dummy = new Dummy();
            setter(dummy, 123);
            Assert.AreEqual(dummy.IntField, 123);
        }

        [Test]
        public void CreatesUntypedSetterByFieldForReferenceType()
        {
            var setter = Field(nameof(Dummy.StringField)).CreateUntypedSetter();
            var dummy = new Dummy();
            setter(dummy, "string");
            Assert.AreEqual(dummy.StringField, "string");
        }

        #endregion

        #region Utilities

        public static FieldInfo Field(string name) => typeof(Dummy).GetField(name);

        public static PropertyInfo Property(string name) => typeof(Dummy).GetProperty(name);

        #endregion
    }
}
