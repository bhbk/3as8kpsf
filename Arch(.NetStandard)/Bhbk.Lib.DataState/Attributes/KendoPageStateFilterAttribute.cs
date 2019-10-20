using System;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Models.KendoPageState;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class KendoPageStateFilterAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value.GetType() != typeof(KendoPageStateFilters))
                return new ValidationResult(this.ErrorMessage);

            var filter = value as KendoPageStateFilters;

            if (filter.Filters == null || filter.Filters.Count == 0)
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
