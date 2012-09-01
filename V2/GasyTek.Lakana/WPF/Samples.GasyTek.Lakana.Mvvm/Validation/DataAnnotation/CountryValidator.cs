using System.ComponentModel.DataAnnotations;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.DataAnnotation
{
    /// <summary>
    /// A custom validator class that can used by System.ComponentModel.DataAnnotations.CustomValidation.
    /// See : http://msdn.microsoft.com/fr-fr/library/system.componentmodel.dataannotations.customvalidationattribute%28v=vs.100%29.aspx
    /// </summary>
    public class CountryValidator
    {
        public static ValidationResult ValidateCountry(Country country, ValidationContext context)
        {
            // simulate a custom validation.
            return country.Id != 4 ? new ValidationResult("Country other than Madagascar is not allowed.") : ValidationResult.Success;
        }
    }
}