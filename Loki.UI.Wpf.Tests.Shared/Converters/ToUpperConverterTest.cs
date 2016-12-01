using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

using Loki.UI.Wpf.Converters;

using Xunit;

namespace Loki.UI.Wpf.Tests.Converters
{
    [Trait("Category", "WPF Converters")]
    public class ToUpperConverterTest
    {
        private readonly IValueConverter converter;

        public ToUpperConverterTest()
        {
                this.converter = new ToUpperConverter();
        }

        [Fact(DisplayName = "ToUpperConverter - BackUnset")]
        public void ConvertBackUnset()
        {
            var value = this.converter.ConvertBack("Pouet", typeof(string), null, CultureInfo.CurrentCulture);
            Assert.Equal(DependencyProperty.UnsetValue, value);
        }

        [Fact(DisplayName = "ToUpperConverter - Convert")]
        public void Convert()
        {
            var value = this.converter.Convert("Pouet", typeof(string), null, CultureInfo.CurrentCulture);
            Assert.Equal("POUET", value);
        }
    }
}
