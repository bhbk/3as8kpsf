using Bhbk.Lib.DataState.Models;
using System;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Extensions.DataStateExtensions;

namespace Bhbk.Lib.DataState.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DataStateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is Models.PagerState model)
            {
                if (model.Filter != null
                    && !IsFilterValid(model.Filter))
                    return new ValidationResult(this.ErrorMessage);

                if (!IsSortValid(model.Sort))
                    return new ValidationResult(this.ErrorMessage);

                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
