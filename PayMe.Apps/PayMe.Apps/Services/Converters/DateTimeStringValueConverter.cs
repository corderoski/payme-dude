using System;
using System.Globalization;
using Xamarin.Forms;

namespace PayMe.Apps.Services.Converters
{

    public class DateTimeStringValueConverter : IValueConverter
    {

        public const string DATE_TIME_FORMAT = "g";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime.TryParse(value + "", out DateTime parsedDate);

                return parsedDate > DateTime.MinValue ? parsedDate.ToString(DATE_TIME_FORMAT) : value;
            }
            if (value is DateTimeOffset)
            {
                DateTimeOffset.TryParse(value + "", out DateTimeOffset parsedDate);

                return parsedDate > DateTimeOffset.MinValue ? parsedDate.ToString(DATE_TIME_FORMAT) : value;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
