using System;

namespace GasyTek.Lakana.Navigation.Attributes
{
    /// <summary>
    /// Attribute taht can be used to attach a unique key to a view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ViewKeyAttribute : Attribute
    {
        private readonly string _uniqueKey;

        public ViewKeyAttribute(string uniqueKey)
        {
            _uniqueKey = uniqueKey;
        }

        public string UniqueKey
        {
            get { return _uniqueKey; }
        }

        public bool AllowMultipleInstances { get; set; }
    }
}