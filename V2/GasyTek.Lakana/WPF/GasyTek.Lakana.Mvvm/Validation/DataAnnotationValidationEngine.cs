using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace GasyTek.Lakana.Mvvm.Validation
{
    /// <summary>
    /// Validation engine for view models that uses DataAnnotations as validation mechanism.
    /// </summary>
    public class DataAnnotationValidationEngine : ValidationEngineBase
    {
        protected override void OnValidateAsync(PropertyInfo property, object propertyValue)
        {
            var validationAttributes = property.GetCustomAttributes(true).OfType<ValidationAttribute>().ToList();
            if (validationAttributes.Any())
            {
                // uses the validator to check wether the property is valid or not
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(this, null, null);
                Validator.TryValidateValue(propertyValue, validationContext, validationResults, validationAttributes);

                if (validationResults.Any())
                {
                    // adds all detected errors for this property
                    var propertyErrors = validationResults.Select(v => v.ErrorMessage).ToList();
                    Errors.AddOrUpdate(property.Name, propertyErrors, (key, currentValue) => propertyErrors);
                }
                else
                {
                    // reset errors for this property
                    List<string> propertyErrors;
                    Errors.TryRemove(property.Name, out propertyErrors);
                }

                OnRaiseErrorsChangedEvent(property.Name);
            }
        }
    }
}
