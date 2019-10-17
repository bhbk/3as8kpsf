using System;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Models.PageState;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PageStateFilterAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value.GetType() != typeof(RecursiveFilterModel))
                return new ValidationResult(this.ErrorMessage);

            var filter = value as RecursiveFilterModel;

            if (filter.Filters == null || filter.Filters.Count == 0)
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
