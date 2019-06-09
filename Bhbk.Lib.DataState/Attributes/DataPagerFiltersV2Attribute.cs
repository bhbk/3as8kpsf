using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bhbk.Lib.DataState.Attributes
{
    public class DataPagerFiltersV2Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            return ValidationResult.Success;
        }
    }
}
