using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;

namespace kalexi.Expressions.Utilities.Tests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
    [SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    public class ExpressionUtilitiesTests
    {
        private const int expectedInt = 123;
        private const string expectedString = "123";

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

        #region Tests for cases when entity type and property type are known

        [Test]
        public void HavingOnlyTheEntityType_RetrievesReferenceTypePropertyInfo()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringProperty;

            PropertyInfo actual = expression.GetProperty();

            Assert.That(actual, Is.EqualTo(GetPropertyByName<Dummy>(nameof(Dummy.StringProperty))));
        }

        [Test]
        public void HavingOnlyTheEntityType_RetrievesValueTypePropertyInfo()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntProperty;

            PropertyInfo actual = expression.GetProperty();

            Assert.That(actual, Is.EqualTo(GetPropertyByName<Dummy>(nameof(Dummy.IntProperty))));
        }

        [Test]
        public void HavingOnlyTheEntityType_RetrievesReferenceTypeFieldInfo()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringField;

            FieldInfo actual = expression.GetField();

            Assert.That(actual, Is.EqualTo(GetFieldByName<Dummy>(nameof(Dummy.StringField))));
        }

        [Test]
        public void HavingOnlyTheEntityType_RetrievesValueTypeFieldInfo()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntField;

            FieldInfo actual = expression.GetField();

            Assert.That(actual, Is.EqualTo(GetFieldByName<Dummy>(nameof(Dummy.IntField))));
        }

        [Test]
        public void HavingOnlyTheEntityType_CreatesWorkingGetterForReferenceTypeProperty()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringProperty;
            Func<Dummy, string> getter = expression.CreateGetter();

            Dummy dummy = new Dummy {StringProperty = expectedString};
            string actualString = getter(dummy);

            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void HavingOnlyTheEntityType_CreatesWorkingGetterForValueTypeProperty()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntProperty;
            Func<Dummy, int> getter = expression.CreateGetter();

            Dummy dummy = new Dummy {IntProperty = expectedInt};
            int actualInt = getter(dummy);

            Assert.AreEqual(expectedInt, actualInt);
        }

        [Test]
        public void HavingOnlyTheEntityType_CreatesWorkingGetterForReferenceTypeField()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringField;
            Func<Dummy, string> getter = expression.CreateGetter();

            Dummy dummy = new Dummy {StringField = expectedString};
            string actualString = getter(dummy);

            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void HavingOnlyTheEntityType_CreatesWorkingGetterForValueTypeField()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntField;
            Func<Dummy, int> getter = expression.CreateGetter();

            Dummy dummy = new Dummy {IntField = expectedInt};
            int actualInt = getter(dummy);

            Assert.AreEqual(expectedInt, actualInt);
        }

        [Test]
        public void HavingOnlyTheEntityType_CreatesWorkingSetterForReferenceTypeProperty()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringProperty;
            Action<Dummy, string> setter = expression.CreateSetter();

            Dummy dummy = new Dummy();
            setter(dummy, expectedString);

            Assert.AreEqual(expectedString, dummy.StringProperty);
        }

        [Test]
        public void HavingOnlyTheEntityType_CreatesWorkingSetterForValueTypeProperty()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntProperty;
            Action<Dummy, int> setter = expression.CreateSetter();

            Dummy dummy = new Dummy();
            setter(dummy, expectedInt);

            Assert.AreEqual(expectedInt, dummy.IntProperty);
        }

        [Test]
        public void HavingOnlyTheEntityType_CreatesWorkingSetterForReferenceTypeField()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringField;
            Action<Dummy, string> setter = expression.CreateSetter();

            Dummy dummy = new Dummy();
            setter(dummy, expectedString);

            Assert.AreEqual(expectedString, dummy.StringField);
        }

        [Test]
        public void HavingOnlyTheEntityType_CreatesWorkingSetterForValueTypeField()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntField;
            Action<Dummy, int> setter = expression.CreateSetter();

            Dummy dummy = new Dummy();
            setter(dummy, expectedInt);

            Assert.AreEqual(expectedInt, dummy.IntField);
        }

        #endregion

        #region Tests for cases when only the entity type is known and property type is unknown

        [Test]
        public void HavingBothEntityAndPropertyTypes_RetrievesReferenceTypePropertyInfo()
        {
            Expression<Func<Dummy, object>> expression = x => x.StringProperty;

            PropertyInfo actual = expression.GetProperty();

            Assert.That(actual, Is.EqualTo(GetPropertyByName<Dummy>(nameof(Dummy.StringProperty))));
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_RetrievesValueTypePropertyInfo()
        {
            Expression<Func<Dummy, object>> expression = x => x.IntProperty;

            PropertyInfo actual = expression.GetProperty();

            Assert.That(actual, Is.EqualTo(GetPropertyByName<Dummy>(nameof(Dummy.IntProperty))));
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_RetrievesReferenceTypeFieldInfo()
        {
            Expression<Func<Dummy, object>> expression = x => x.StringField;

            FieldInfo actual = expression.GetField();

            Assert.That(actual, Is.EqualTo(GetFieldByName<Dummy>(nameof(Dummy.StringField))));
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_RetrievesValueTypeFieldInfo()
        {
            Expression<Func<Dummy, object>> expression = x => x.IntField;

            FieldInfo actual = expression.GetField();

            Assert.That(actual, Is.EqualTo(GetFieldByName<Dummy>(nameof(Dummy.IntField))));
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_CreatesWorkingGetterForReferenceTypeProperty()
        {
            Expression<Func<Dummy, object>> expression = x => x.StringProperty;
            PropertyInfo property = expression.GetProperty();
            Func<Dummy, object> getter = property.CreateGetter<Dummy>();

            Dummy dummy = new Dummy {StringProperty = expectedString};
            object actualString = getter(dummy);

            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_CreatesWorkingGetterForValueTypeProperty()
        {
            Expression<Func<Dummy, object>> expression = x => x.IntProperty;
            PropertyInfo property = expression.GetProperty();
            Func<Dummy, object> getter = property.CreateGetter<Dummy>();

            Dummy dummy = new Dummy {IntProperty = expectedInt};
            object actualInt = getter(dummy);

            Assert.AreEqual(expectedInt, actualInt);
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_CreatesWorkingGetterForReferenceTypeField()
        {
            Expression<Func<Dummy, object>> expression = x => x.StringField;
            FieldInfo field = expression.GetField();
            Func<Dummy, object> getter = field.CreateGetter<Dummy>();

            Dummy dummy = new Dummy {StringField = expectedString};
            object actualString = getter(dummy);

            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_CreatesWorkingGetterForValueTypeField()
        {
            Expression<Func<Dummy, object>> expression = x => x.IntField;
            FieldInfo field = expression.GetField();
            Func<Dummy, object> getter = field.CreateGetter<Dummy>();

            Dummy dummy = new Dummy {IntField = expectedInt};
            object actualInt = getter(dummy);

            Assert.AreEqual(expectedInt, actualInt);
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_CreatesWorkingSetterForReferenceTypeProperty()
        {
            Expression<Func<Dummy, object>> expression = x => x.StringProperty;
            PropertyInfo property = expression.GetProperty();
            Action<Dummy, object> setter = property.CreateSetter<Dummy>();

            Dummy dummy = new Dummy();
            setter(dummy, expectedString);

            Assert.AreEqual(expectedString, dummy.StringProperty);
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_CreatesWorkingSetterForValueTypeProperty()
        {
            Expression<Func<Dummy, object>> expression = x => x.IntProperty;
            PropertyInfo property = expression.GetProperty();
            Action<Dummy, object> setter = property.CreateSetter<Dummy>();

            Dummy dummy = new Dummy();
            setter(dummy, expectedInt);

            Assert.AreEqual(expectedInt, dummy.IntProperty);
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_CreatesWorkingSetterForReferenceTypeField()
        {
            Expression<Func<Dummy, object>> expression = x => x.StringField;
            FieldInfo field = expression.GetField();
            Action<Dummy, object> setter = field.CreateSetter<Dummy>();

            Dummy dummy = new Dummy();
            setter(dummy, expectedString);

            Assert.AreEqual(expectedString, dummy.StringField);
        }

        [Test]
        public void HavingBothEntityAndPropertyTypes_CreatesWorkingSetterForValueTypeField()
        {
            Expression<Func<Dummy, object>> expression = x => x.IntField;
            FieldInfo field = expression.GetField();
            Action<Dummy, object> setter = field.CreateSetter<Dummy>();

            Dummy dummy = new Dummy();
            setter(dummy, expectedInt);

            Assert.AreEqual(expectedInt, dummy.IntField);
        }

        #endregion

        #region Tests for cases when nothing is available in strongly-typed manner

        [Test]
        public void HavingNothingInCompileTime_CreatesWorkingGetterForReferenceTypeProperty()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringProperty;
            PropertyInfo property = expression.GetProperty();
            Func<object, object> getter = property.CreateUntypedGetter();

            Dummy dummy = new Dummy {StringProperty = expectedString};
            object actualString = getter(dummy);

            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void HavingNothingInCompileTime_CreatesWorkingGetterForValueTypeProperty()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntProperty;
            PropertyInfo property = expression.GetProperty();
            Func<object, object> getter = property.CreateUntypedGetter();

            Dummy dummy = new Dummy {IntProperty = expectedInt};
            object actualInt = getter(dummy);

            Assert.AreEqual(expectedInt, actualInt);
        }

        [Test]
        public void HavingNothingInCompileTime_CreatesWorkingGetterForReferenceTypeField()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringField;
            FieldInfo field = expression.GetField();
            Func<object, object> getter = field.CreateUntypedGetter();

            Dummy dummy = new Dummy {StringField = expectedString};
            object actualString = getter(dummy);

            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void HavingNothingInCompileTime_CreatesWorkingGetterForValueTypeField()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntField;
            FieldInfo field = expression.GetField();
            Func<object, object> getter = field.CreateUntypedGetter();

            Dummy dummy = new Dummy {IntField = expectedInt};
            object actualInt = getter(dummy);

            Assert.AreEqual(expectedInt, actualInt);
        }

        [Test]
        public void HavingNothingInCompileTime_CreatesWorkingSetterForReferenceTypeProperty()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringProperty;
            PropertyInfo property = expression.GetProperty();
            Action<object, object> setter = property.CreateUntypedSetter();

            Dummy dummy = new Dummy();
            setter(dummy, expectedString);

            Assert.AreEqual(expectedString, dummy.StringProperty);
        }

        [Test]
        public void HavingNothingInCompileTime_CreatesWorkingSetterForValueTypeProperty()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntProperty;
            PropertyInfo property = expression.GetProperty();
            Action<object, object> setter = property.CreateUntypedSetter();

            Dummy dummy = new Dummy();
            setter(dummy, expectedInt);

            Assert.AreEqual(expectedInt, dummy.IntProperty);
        }

        [Test]
        public void HavingNothingInCompileTime_CreatesWorkingSetterForReferenceTypeField()
        {
            Expression<Func<Dummy, string>> expression = x => x.StringField;
            FieldInfo field = expression.GetField();
            Action<object, object> setter = field.CreateUntypedSetter();

            Dummy dummy = new Dummy();
            setter(dummy, expectedString);

            Assert.AreEqual(expectedString, dummy.StringField);
        }

        [Test]
        public void HavingNothingInCompileTime_CreatesWorkingSetterForValueTypeField()
        {
            Expression<Func<Dummy, int>> expression = x => x.IntField;
            FieldInfo field = expression.GetField();
            Action<object, object> setter = field.CreateUntypedSetter();

            Dummy dummy = new Dummy();
            setter(dummy, expectedInt);

            Assert.AreEqual(expectedInt, dummy.IntField);
        }

        #endregion

        #region Tests for retrieving method info

        [Test]
        public void HavingEntityType_RetrievesMethodInfoByVoidMethodExpression()
        {
            Expression<Action<Dummy>> expression = x => x.VoidMethod();

            MethodInfo methodInfo = expression.GetMethodInfo();

            Assert.That(methodInfo, Is.EqualTo(GetMethodbyName<Dummy>(nameof(Dummy.VoidMethod))));
        }

        [Test]
        public void HavingEntityType_RetrievesMethodInfoByReferenceTypeMethodExpression()
        {
            Expression<Func<Dummy, object>> expression = x => x.StringMethod();

            MethodInfo methodInfo = expression.GetMethodInfo();

            Assert.That(methodInfo, Is.EqualTo(GetMethodbyName<Dummy>(nameof(Dummy.StringMethod))));
        }

        [Test]
        public void HavingEntityType_RetrievesMethodInfoByValueTypeMethodExpression()
        {
            Expression<Func<Dummy, object>> expression = x => x.IntMethod();

            MethodInfo methodInfo = expression.GetMethodInfo();

            Assert.That(methodInfo, Is.EqualTo(GetMethodbyName<Dummy>(nameof(Dummy.IntMethod))));
        }

        #endregion

        #region Utilities

        private static FieldInfo GetFieldByName<T>(string name) => typeof(T).GetField(name);

        private static PropertyInfo GetPropertyByName<T>(string name) => typeof(T).GetProperty(name);

        private static MethodInfo GetMethodbyName<T>(string name) => typeof(T).GetMethod(name);

        #endregion
    }
}
