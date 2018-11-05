using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bhbk.Lib.Core.Attributes
{
    public class ListOfStringsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(this.ErrorMessage);

            var list = value as string[];

            if (list.Any(x => string.IsNullOrEmpty(x)))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
