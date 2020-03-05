using Bhbk.Lib.DataState.Models;
using System;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Extensions.PagerV2Extensions;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PagerV2Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is PagerV2 state)
            {
                if (!IsSortValid(state.Sort))
                    return new ValidationResult(this.ErrorMessage);

                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
