using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static Bhbk.Lib.DataState.Models.PageState;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PageStateSortAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<SortModel>))
                return new ValidationResult(this.ErrorMessage);

            var list = value as List<SortModel>;

            if (list.Count == 0
                || list.Any(x => string.IsNullOrEmpty(x.Field))
                || list.Any(x => !x.Dir.Equals("asc") && !x.Dir.Equals("desc")))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
