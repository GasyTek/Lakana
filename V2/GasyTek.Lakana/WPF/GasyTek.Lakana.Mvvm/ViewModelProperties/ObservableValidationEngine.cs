using System;
using GasyTek.Lakana.Mvvm.Validation;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    public class ObjectChangedEventArgs : EventArgs
    {
        public IValidationEngine OldValue { get; set; }
        public IValidationEngine NewValue { get; set; }
    }

    public class ObservableValidationEngine
    {
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;

        private IValidationEngine _object;

        public IValidationEngine Object
        {
            get { return _object; } 
            set
            {
                var oldValue = _object;
                var newValue = value;
                _object = value;
                OnObjectChanged(new ObjectChangedEventArgs { OldValue = oldValue , NewValue = newValue });
            }
        }

        private void OnObjectChanged(ObjectChangedEventArgs e)
        {
            var handler = ObjectChanged;
            if (handler != null) handler(this, e);
        }
    }
}