using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CascadePagerSortAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<KeyValuePair<string, string>>))
                return new ValidationResult(this.ErrorMessage);

            var list = value as List<KeyValuePair<string, string>>;

            if (list.Count == 0
                || list.Any(x => string.IsNullOrEmpty(x.Key))
                || list.Any(x => !x.Value.Equals("asc") && !x.Value.Equals("desc")))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
