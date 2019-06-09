using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bhbk.Lib.DataState.Attributes
{
    public class DataPagerSortsV2Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
                return new ValidationResult(this.ErrorMessage);

            if (value.GetType() != typeof(List<KeyValuePair<string, string>>))
                return new ValidationResult(this.ErrorMessage);

            var list = value as List<KeyValuePair<string, string>>;

            if (list.Any(x => string.IsNullOrEmpty(x.Key)))
                return new ValidationResult(this.ErrorMessage);

            if (list.Any(x => !x.Value.Equals("asc") 
                && !x.Value.Equals("desc")))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
