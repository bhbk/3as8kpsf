using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bhbk.Lib.Core.Attributes
{
    public class CascadePagerOrdersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<Tuple<string, string>>))
                return new ValidationResult(this.ErrorMessage);

            var list = value as List<Tuple<string, string>>;

            if (list.Any(x => string.IsNullOrEmpty(x.Item2)))
                return new ValidationResult(this.ErrorMessage);

            if (list.Any(x => !x.Item2.Equals("asc") 
                && !x.Item2.Equals("desc")))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
