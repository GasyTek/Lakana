using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Common.Utils;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// <see cref="IEnumItem{TEnum}"/>
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    public class EnumItem<TEnum> : IEnumItem<TEnum> where TEnum : struct 
    {
        #region Fields

        private readonly IUIMetadata _uiMetadata;
        private readonly TEnum _value;

        #endregion

        #region Properties

        public IUIMetadata UIMetadata
        {
            get { return _uiMetadata; }
        }

        public TEnum Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Shortcut for label property of presentation metadata.
        /// </summary>
        public string Label
        {
            get { return (UIMetadata != null) ? UIMetadata.Label : GlobalConstants.LocalizationNoText; }
        }

        #endregion

        #region Constructor

        public EnumItem(IUIMetadata uiMetadata, TEnum value)
        {
            _uiMetadata = uiMetadata;
            _value = value;
        }

        #endregion
    }
}