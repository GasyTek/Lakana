using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.Validation;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.Custom
{
    internal class MyCustomValidationEngine : ValidationEngineBase
    {
        private readonly SampleCustomValidationViewModel _viewModel;

        public MyCustomValidationEngine(SampleCustomValidationViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        protected override void OnValidate(ValidationParameter validationParameter)
        {
            // Example of Synchronous validation
            SynchronouslyValidateProperty(validationParameter);

            // Example of asynchronous validation
            //AsynchronouslyValidateProperty(validationParameter);
        }

        #region Synchronous validation

        /// <summary>
        /// Synchronously validates the property.
        /// </summary>
        private void SynchronouslyValidateProperty(ValidationParameter validationParameter)
        {
            var validateCoreResult = ValidateCore(validationParameter);

            // optional properties
            ClearErrorMessage(validateCoreResult.ToClearProperties);

            // main property
            ClearErrorMessage(new[] { validateCoreResult.Property });
            if (!validateCoreResult.IsValid) AppendErrorMessage(validateCoreResult.Property, validateCoreResult.ErrorMessage);

            // always notify that the error has changed to tell the ui to refresh
            OnRaiseErrorsChangedEvent(validateCoreResult.Property, validateCoreResult.ToClearProperties.ToArray());
        }

        #endregion

        #region Asynchronous validation

        /// <summary>
        /// Asynchronously validates the property.
        /// </summary>
        private void AsynchronouslyValidateProperty(ValidationParameter validationParameter)
        {
            var tsk = Task<ValidateCoreResult>.Factory.StartNew(() =>
                                                {
                                                    var validateCoreResult = ValidateCore(validationParameter);

                                                    // optional properties
                                                    ClearErrorMessage(validateCoreResult.ToClearProperties);

                                                    // main property
                                                    ClearErrorMessage(new[] { validateCoreResult.Property });
                                                    if (!validateCoreResult.IsValid) AppendErrorMessage(validateCoreResult.Property, validateCoreResult.ErrorMessage);

                                                    return validateCoreResult;
                                                });
            tsk.ContinueWith(t =>
                                 {
                                     var validateCoreResult = t.Result;
                                     OnRaiseErrorsChangedEvent(validateCoreResult.Property,
                                                               validateCoreResult.ToClearProperties.ToArray());
                                 }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region Common

        private ValidateCoreResult ValidateCore(ValidationParameter validationParameter)
        {
            var propertyMetadata = validationParameter.PropertyMetadata;
            var propertyValue = validationParameter.PropertyValue;

            var validateCoreResult = new ValidateCoreResult { Property = propertyMetadata.Name, IsValid = true };

            if (propertyMetadata.Name == "Code")
            {
                if (string.IsNullOrEmpty((string)propertyValue))
                {
                    validateCoreResult.IsValid = false;
                    validateCoreResult.ErrorMessage = "Code is required";
                }

                var value = (string) propertyValue;
                if (value != null && value.Length > 3)
                {
                    validateCoreResult.IsValid = false;
                    validateCoreResult.ErrorMessage = "Code must not exceed 3 characters.";
                }
            }

            if (propertyMetadata.Name == "Age")
            {
                if (propertyValue != null)
                {
                    var age = (int)propertyValue;
                    if (age < 1 || age > 50)
                    {
                        validateCoreResult.IsValid = false;
                        validateCoreResult.ErrorMessage = "Age must be between 1 and 50.";
                    }
                }
            }

            if (propertyMetadata.Name == "Country")
            {
                var country = (Country)propertyValue;
                if (country != null && country.Id != 4)
                {
                    validateCoreResult.IsValid = false;
                    validateCoreResult.ErrorMessage = "Country other than Madagascar is not allowed.";
                }
            }

            if (propertyMetadata.Name == "DateOfBirth")
            {
                if (propertyValue != null)
                {
                    var dateOfBirth = (DateTime)propertyValue;
                    if (dateOfBirth >= _viewModel.DateOfHire.Value)
                    {
                        validateCoreResult.IsValid = false;
                        validateCoreResult.ErrorMessage = "Date of Birth must preced the Date of Hire";                        
                    }

                    validateCoreResult.ToClearProperties = new List<string> { "DateOfHire" };
                }
            }

            if (propertyMetadata.Name == "DateOfHire")
            {
                if (propertyValue != null)
                {
                    var dateOfHire = (DateTime)propertyValue;
                    if (dateOfHire <= _viewModel.DateOfBirth.Value)
                    {
                        validateCoreResult.IsValid = false;
                        validateCoreResult.ErrorMessage = "Date of Birth must preced the Date of Hire";                       
                    }

                    validateCoreResult.ToClearProperties = new List<string> { "DateOfBirth" };
                }
            }

            return validateCoreResult;
        }

        private void AppendErrorMessage(string propertyName, string message)
        {
            var errorCollection = Errors.GetOrAdd(propertyName, new ErrorCollection());
            errorCollection.AddError(message);
        }

        private void ClearErrorMessage(IEnumerable<string> propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException("propertyNames");
            foreach (var propertyName in propertyNames)
            {
                ErrorCollection errorCollection;
                Errors.TryRemove(propertyName, out errorCollection);
            }
        }

        #endregion

        #region Internal class

        internal class ValidateCoreResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
            public string Property { get; set; }
            public IEnumerable<string> ToClearProperties { get; set; }

            public ValidateCoreResult()
            {
                ToClearProperties = new List<string>();
            }
        }

        #endregion
    }
}