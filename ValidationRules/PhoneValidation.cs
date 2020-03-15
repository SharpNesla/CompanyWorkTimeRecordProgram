using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Employees.ValidationRules
{
    class PhoneValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);
            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Номер телефона не может быть пустым");
            if (!Regex.IsMatch(strValue, @"[0-9*#-()]{6,}"))
            {
                return new ValidationResult(false, $"Неправильный формат номера");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
