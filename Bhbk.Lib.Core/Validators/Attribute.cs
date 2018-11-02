using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bhbk.Lib.Core.Validators
{
    public class RequiredStringArrayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] array = value as string[];

            if (array == null || array.Any(item => string.IsNullOrEmpty(item)))
                return new ValidationResult(this.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
