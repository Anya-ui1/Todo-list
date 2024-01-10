using System.Globalization;
using System.Windows.Data;

namespace TodoList
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            return string.Format("{0:dd-MM-yyyy}", date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value.ToString();
            return DateTime.ParseExact(strValue, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }
    }
}