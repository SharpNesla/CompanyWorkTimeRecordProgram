using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Employees.ValidationRules
{
    public class PersonTextDataValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public bool IsRequired { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);
            if (string.IsNullOrEmpty(strValue) && IsRequired)
                return new ValidationResult(false, $"{FieldName} не может быть пустым");
            if (!Regex.IsMatch(strValue, @"^[а-яА-Я-а-яА-Я ]*([а-я])$"))
            {
                return new ValidationResult(false, $"{FieldName} содержит не правильные символы");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}