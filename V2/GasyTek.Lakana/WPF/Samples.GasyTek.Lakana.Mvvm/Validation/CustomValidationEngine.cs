using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.Validation;

namespace Samples.GasyTek.Lakana.Mvvm.Validation
{
    [Export(typeof(IValidationEngine))]
    internal class CustomValidationEngine : ValidationEngineBase
    {
        protected override void OnValidateAsync(PropertyInfo property, object propertyValue)
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