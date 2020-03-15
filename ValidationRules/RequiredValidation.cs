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
    public class RequiredValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);
            if (string.IsNullOrEmpty(strValue))
            {
                return new ValidationResult(false, $"{FieldName} не может быть пустым");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
