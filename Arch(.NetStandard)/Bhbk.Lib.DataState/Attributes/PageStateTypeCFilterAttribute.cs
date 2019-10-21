using System;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Expressions.PageStateTypeCStateExtensions;
using static Bhbk.Lib.DataState.Models.PageStateTypeC;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PageStateTypeCFilterAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value.GetType() != typeof(PageStateTypeCFilters))
                return new ValidationResult(this.ErrorMessage);

            var filter = value as PageStateTypeCFilters;

            if (!IsFilterValid(filter))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
