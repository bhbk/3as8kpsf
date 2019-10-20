using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Models.KendoPageState;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class KendoPageStateSortAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<KendoPageStateSort>))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
