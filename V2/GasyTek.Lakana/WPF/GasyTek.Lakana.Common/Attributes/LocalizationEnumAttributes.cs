using System;

namespace GasyTek.Lakana.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class LocalizationEnumAttribute : Attribute
    {
        private readonly string _localizationID;

        /// <summary>
        /// The id of the localized string in the resource file.
        /// </summary>
        public string LocalizationID
        {
            get { return _localizationID; }
        }

        public LocalizationEnumAttribute(string localizationID)
        {
            _localizationID = localizationID;
        }
    }
}
