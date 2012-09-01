using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.Validation;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace Samples.GasyTek.Lakana.Mvvm.Validation.Custom
{
    [Export(typeof(IValidationEngine))]
    internal class CustomValidationEngine : ValidationEngineBase
    {
        protected override void OnValidate(PropertyInfo property, object propertyValue)
        {
            var tsk = Task<bool>.Factory.StartNew(() =>
                                                      {
                                                          Thread.Sleep(100);
                                                          var propertyErrors = new List<string>() { DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) };
                                                          Errors.AddOrUpdate(property.Name, propertyErrors, (key, oldValue) => propertyErrors);
                                                          return false;
                                                      });
            tsk.ContinueWith(t => OnRaiseErrorsChangedEvent(property.Name), TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}