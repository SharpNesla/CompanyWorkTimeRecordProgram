using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Employees.ValidationRules
{
    public class DecimalValidationRule : ValidationRule
    {
        public Type ValidationType { get; set; }

        public string FieldName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Поле {FieldName} не может быть незаполненным.");
            decimal decimalVal = 0;
            var canConvert = decimal.TryParse(strValue, out decimalVal);
            return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Поле {FieldName} содержит" +
                                                                                               $" символы, отличные от цифр и знака разделителя.");
        }
    }
}
