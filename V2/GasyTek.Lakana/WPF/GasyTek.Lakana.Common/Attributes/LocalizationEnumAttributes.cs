using System;

namespace GasyTek.Lakana.Common.Attributes
{
    /// <summary>
    /// Decorates enum member with this metadata to indicate the resource id corresponding to the member.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationEnumAttribute"/> class.
        /// </summary>
        /// <param name="localizationID">The localization ID.</param>
        public LocalizationEnumAttribute(string localizationID)
        {
            _localizationID = localizationID;
        }
    }
}
