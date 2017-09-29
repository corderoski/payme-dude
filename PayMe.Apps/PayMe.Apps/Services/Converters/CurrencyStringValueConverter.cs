using System;
using System.Globalization;
using Xamarin.Forms;

namespace PayMe.Apps.Services.Converters
{

    public class CurrencyStringValueConverter : IValueConverter
    {

        public const string CURRENCY_STRING_FORMAT = "C";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
            {
                return ((decimal)value).ToString(CURRENCY_STRING_FORMAT);
            }
            if (value is float)
            {
                return ((float)value).ToString(CURRENCY_STRING_FORMAT);
            }
            if (value is int)
            {
                return ((int)value).ToString(CURRENCY_STRING_FORMAT);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
