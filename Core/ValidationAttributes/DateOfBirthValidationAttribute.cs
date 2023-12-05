using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValidationAttributes
{
    public class DateOfBirthValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                int currentYear = DateTime.Now.Year;

                if (dateOfBirth.Year > currentYear)
                {
                    return new ValidationResult($"The Doctor can not be born in the future: currentYear = {currentYear}, entered year = {dateOfBirth.Year}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
