using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Attributes
{
    public class DataStateSortAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is ICollection<IDataStateSort> sort)
            {
                if (!CheckSort(sort))
                    return new ValidationResult("The sort is invalid");
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Checks for invalid direction values. If any sort descriptor in the collection contains
        /// an invalid Dir value, the validation fails. The Direction must be either asc or desc.
        /// </summary>
        /// <returns>true if the collection does not contain any invalid values; otherwise false</returns>
        private bool CheckSort(ICollection<IDataStateSort> sort)
            => PagerStateSort.TryParse(sort, out ICollection<IDataStateSort> _sort);
    }
}
