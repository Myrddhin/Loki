using System;

using Loki.Common.Diagnostics.Tests;

using Xunit;

namespace Loki.Common.Tools
{
    [Trait("Category", "Tools")]
    public class ExpressionHelperTest
    {
        [Fact(DisplayName = "Helper : public constructor test (valid type)")]
        public void HasPublicConstructor()
        {
            Assert.True(ExpressionHelper.HasDefaultConstructor(typeof(Exception)));
        }

        [Fact(DisplayName = "Helper : public constructor test (invalid type)")]
        public void HasNotPublicConstructor()
        {
            Assert.False(ExpressionHelper.HasDefaultConstructor(typeof(InvalidException)));
        }

        [Fact(DisplayName = "Helper : new functor (valid type)")]
        public void NewWithPublicConstructor()
        {
            var builder = ExpressionHelper.New<Exception>();
            var pouet = builder.Compile()();

            Assert.IsType<Exception>(pouet);
        }

        [Fact(DisplayName = "Helper : new functor (invalid type)")]
        public void NewWithoutPublicConstructor()
        {
            Assert.Throws<ArgumentException>(() => ExpressionHelper.New<InvalidException>());
        }

        [Fact(DisplayName = "Helper : get propery type (reference property type)")]
        public void GetPropertyDirect()
        {
            var pouet = ExpressionHelper.GetProperty<Exception, string>(x => x.Message);
            Assert.Equal("Message", pouet.Name);
        }

        [Fact(DisplayName = "Helper : get propery type (value property type)")]
        public void GetPropertyWithBoxing()
        {
            var pouet = ExpressionHelper.GetProperty<Exception, object>(x => x.HResult);
            Assert.Equal("HResult", pouet.Name);
        }
    }
}