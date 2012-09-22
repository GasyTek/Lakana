using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GasyTek.Lakana.Mvvm.Validation
{
    /// <summary>
    /// Validation engine for view models that uses DataAnnotations as validation mechanism.
    /// </summary>
    public class DataAnnotationValidationEngine : ValidationEngineBase
    {
        protected override void OnValidate(ValidationParameter validationParameter)
        {
            Task.Factory.StartNew(() => ValidateAsync(validationParameter))
                .ContinueWith(t =>
                        {
                            if (t.Result)
                                OnRaiseErrorsChangedEvent(validationParameter.PropertyMetadata.Name);
                        },
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        private bool ValidateAsync(ValidationParameter validationParameter)
        {
            var propertyMetadata = validationParameter.PropertyMetadata;
            var propertyValue = validationParameter.PropertyValue;

            var validationAttributes = propertyMetadata.GetCustomAttributes(true).OfType<ValidationAttribute>().ToList();
            if (validationAttributes.Any())
            {
                // uses the validator to check wether the property is valid or not
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(this, null, null);
                Validator.TryValidateValue(propertyValue, validationContext, validationResults, validationAttributes);

                if (validationResults.Any())
                {
                    // adds all detected errors for this property
                    var errorCollection = Errors.GetOrAdd(propertyMetadata.Name, new ErrorCollection());
                    errorCollection.Clear();
                    errorCollection.AddErrors(validationResults.Select(v => v.ErrorMessage));
                    Errors.GetOrAdd(propertyMetadata.Name, new ErrorCollection(validationResults.Select(v => v.ErrorMessage)));
                }
                else
                {
                    // reset errors for this property
                    ErrorCollection errorCollection;
                    Errors.TryRemove(propertyMetadata.Name, out errorCollection);
                }
                return true;
            }
            return false;
        }
    }
}
