using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static Bhbk.Lib.DataState.Models.DataPagerV3;

namespace Bhbk.Lib.DataState.Attributes
{
    public class DataPagerSortsV3Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<SortDescriptor>))
                return new ValidationResult(this.ErrorMessage);

            var list = value as List<SortDescriptor>;

            if (list.Any(x => string.IsNullOrEmpty(x.Field)))
                return new ValidationResult(this.ErrorMessage);

            if (list.Any(x => !x.Dir.Equals("asc")
                && !x.Dir.Equals("desc")))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
