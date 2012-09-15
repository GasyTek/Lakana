using System;
using System.Collections.Generic;
using System.Linq;
using GasyTek.Lakana.Common.UI;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    /// <summary>
    /// A view model property used to expose enum types.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    public class EnumViewModelProperty<TEnum>
        : LookupViewModelProperty<TEnum, IEnumItem<TEnum>>, IEnumViewModelProperty<TEnum> where TEnum : struct
    {
        #region Fields

        private readonly Func<TEnum, IUIMetadata> _enumUIMetadataProvider;

        #endregion

        #region Constructor

        internal EnumViewModelProperty(TEnum originalValue, Func<TEnum, IUIMetadata> enumUIMetadataProvider, ObservableValidationEngine internalObservableValidationEngine)
            : base(originalValue, null, internalObservableValidationEngine)
        {
            _enumUIMetadataProvider = enumUIMetadataProvider;
            Initialize(originalValue);
        }

        #endregion

        #region Initialization

        private void Initialize(TEnum originalValue)
        {
            // initialize the list of all possible values
            var properties = new List<IEnumItem<TEnum>>();
            var enumMemberValues = (TEnum[])Enum.GetValues(typeof(TEnum));
            if (_enumUIMetadataProvider != null)
            {
                properties.AddRange((from value in enumMemberValues 
                                     let uiMetadata = _enumUIMetadataProvider(value) 
                                     select new EnumItem<TEnum>(uiMetadata, value)));
            }
            else
            {
                properties.AddRange((from value in enumMemberValues
                                     let uiMetadata = new UIMetadata {LabelProvider = value.ToString}
                                     select new EnumItem<TEnum>(uiMetadata, value)));
            }

            // fills the enum members
            AssignItemsSourceProvider(() => properties);

            // set the original selected value from the list
            AssignOriginalValue(properties.Select(p => p.Value).First(e => e.Equals(originalValue)));
        }

        #endregion
    }
}
