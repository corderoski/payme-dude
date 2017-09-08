using System;
using System.ComponentModel.DataAnnotations;

namespace PayMe.Framework.Data.Annotations
{
    /// <summary>
    /// Validates that a value must be greater than the specified Min
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ValidNumericValue : ValidationAttribute
    {

        private readonly Type _type;
        private readonly int _minIntAllowedValue;

        public ValidNumericValue(int min = 0)
        {
            _minIntAllowedValue = min;
        }

        public ValidNumericValue(int min, Type type)
        {
            _minIntAllowedValue = min;
            _type = type;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult result = null;
            if (value is int || value is long || value is short)
            {
                var tmp = (int)value;
                result = tmp > _minIntAllowedValue ?
                    ValidationResult.Success : new ValidationResult("Must indicate a value.");
            }
            else if (value is float tmpFloat)
            {
                result = tmpFloat > _minIntAllowedValue ?
                    ValidationResult.Success : new ValidationResult("Must indicate a value.");
            }
            else if (value is decimal tmpDecimal)
            {
                result = tmpDecimal > _minIntAllowedValue ? 
                    ValidationResult.Success : new ValidationResult("Must indicate a value.");
            }
            else if (value is Enum)
            {
                result = Enum.IsDefined(_type, value) ? ValidationResult.Success : new ValidationResult("Must indicate a value.");
            }
            else
            {
                result = new ValidationResult("Invalid value.");
            }

            return result;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Must select a valid value for '{name}'.";
        }

    }
}
