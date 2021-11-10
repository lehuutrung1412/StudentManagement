using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StudentManagement.Utils
{
    public class NotEmptyValidationRule: ValidationRule
    {
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(false, ErrorMessage ?? "A value is required");
            }
            return ValidationResult.ValidResult;
        }

    }
}
