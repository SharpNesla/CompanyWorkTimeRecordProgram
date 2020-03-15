using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Employees.ValidationRules
{
    class SearchRequiredValidation : ValidationRule
    {
        public string FieldName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
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
