using System;

namespace GasyTek.Lakana.Mvvm.Validation
{
    public class ErrorsChangedEventArgs : EventArgs
    {
        public string PropertyName { get; private set; }

        public ErrorsChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    public delegate void ErrorsChangedEventHandler (object sender, ErrorsChangedEventArgs e);
}