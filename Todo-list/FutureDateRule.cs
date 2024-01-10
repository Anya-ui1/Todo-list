using System.Globalization;
using System.Windows.Controls;

namespace TodoList
{
    internal class FutureDateRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime date;
            try
            {
                date = DateTime.Parse(value.ToString());
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Value is not a valid date. Please enter a valid date");
            }
            if (DateTime.Now.Date > date)
            {
                return new ValidationResult(false, "Value is not a future date. Please enter a date in the future.");
            }
            return ValidationResult.ValidResult;
        }

    }
}
