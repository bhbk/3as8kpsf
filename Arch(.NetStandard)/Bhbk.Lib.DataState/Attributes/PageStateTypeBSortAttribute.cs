using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Expressions.PageStateTypeBExtensions;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PageStateTypeBSortAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<KeyValuePair<string, string>>))
                return new ValidationResult(this.ErrorMessage);

            var sort = value as List<KeyValuePair<string, string>>;

            if (!IsSortValid(sort))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
