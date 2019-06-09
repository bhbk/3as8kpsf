using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Bhbk.Lib.DataState.Models.DataPagerV3;

namespace Bhbk.Lib.DataState.Attributes
{
    public class DataPagerFiltersV3Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<FilterDescriptor>))
                return new ValidationResult(this.ErrorMessage);

            var list = value as List<FilterDescriptor>;

            if(list.Count == 0)
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
