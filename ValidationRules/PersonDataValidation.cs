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
    /// <summary>
    /// Валидатор, проверяющий поле на заполненность и соответствие русскому алфавиту и символам пробела.
    /// </summary>
    public class PersonTextDataValidation : ValidationRule
    {
        public string FieldName { get; set; }

        public bool IsRequired { get; set; } = true;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = value?.ToString();
            if (string.IsNullOrWhiteSpace(strValue))
            {
                if (IsRequired)
                {
                    return new ValidationResult(false, $"Поле {FieldName} не может быть незаполненным");
                }
                return new ValidationResult(true, null);
            }

            if (!Regex.IsMatch(strValue, @"^[а-яА-Я-а-яА-Я ё]*([а-я])$"))
            {
                return new ValidationResult(false, $"Поле {FieldName} содержит некорректные символы или не заполнено");
            }

            return new ValidationResult(true, null);
        }
    }
}