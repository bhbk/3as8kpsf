using Bhbk.Lib.DataState.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bhbk.Lib.DataState.Attributes
{
    public class DataStateFilterAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validation for IDataStateFilter
        /// </summary>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IDataStateFilter filter)
            {
                if (!CheckFilter(filter))
                    return new ValidationResult("The filter is invalid");
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Recursively checks all decendants of a Filter. If any decendant Filter is not valid,
        /// the validation fails. The Field and Operator values for each Filter may either both
        /// have values or both be null, but one may not contain a value and the other be null.
        /// </summary>
        private bool CheckFilter(IDataStateFilter filter)
        {
            bool result = filter?.Filters?.ToList()?
                .All(f => CheckFilter(f)) ?? true;

            if (string.IsNullOrWhiteSpace(filter?.Field)
             && string.IsNullOrWhiteSpace(filter?.Operator)
             && !string.IsNullOrWhiteSpace(filter?.Value))
                return false;
            if (string.IsNullOrWhiteSpace(filter?.Field)
             && string.IsNullOrWhiteSpace(filter?.Operator))
                return result;
            else if (!string.IsNullOrWhiteSpace(filter?.Field)
             && !string.IsNullOrWhiteSpace(filter?.Operator))
                return result;

            return false;
        }
    }
}
