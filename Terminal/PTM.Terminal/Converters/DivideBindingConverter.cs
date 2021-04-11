using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace PTM.Terminal.Converters
{
    /// <summary>
    /// Mnoży zbindowaną wartość przez parametr.
    /// Używany w xaml.
    /// </summary>
    public class DivideBindingConverter : MarkupExtension, IValueConverter
    {
        private static DivideBindingConverter mInstance;

        /// <summary>
        /// Mnoży zbindowaną wartość przez parametr
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
        }

        /// <summary>
        /// Funkcja odwrtona, nie funkcjonuje
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Instancjuje konwerter
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mInstance ?? (mInstance = new DivideBindingConverter());
        }
    }
}