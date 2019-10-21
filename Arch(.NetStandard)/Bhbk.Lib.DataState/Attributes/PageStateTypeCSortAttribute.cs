using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Expressions.PageStateTypeCStateExtensions;
using static Bhbk.Lib.DataState.Models.PageStateTypeC;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PageStateTypeCSortAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<PageStateTypeCSort>))
                return new ValidationResult(this.ErrorMessage);

            var sort = value as List<PageStateTypeCSort>;

            if (!IsSortValid(sort))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
