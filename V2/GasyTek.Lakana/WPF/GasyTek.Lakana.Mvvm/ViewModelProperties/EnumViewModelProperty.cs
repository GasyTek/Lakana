using System;
using System.Collections.Generic;
using System.Linq;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Common.Utils;

namespace GasyTek.Lakana.Mvvm.ViewModelProperties
{
    public class EnumViewModelProperty<TEnum>
        : LookupViewModelProperty<TEnum, IEnumItem<TEnum>>, IEnumViewModelProperty<TEnum> where TEnum : struct
    {
        #region Fields

        private readonly Type _localizationResourceType;

        #endregion

        #region Constructor

        internal EnumViewModelProperty(TEnum originalValue, Type localizationResourceType, ObservableValidationEngine internalObservableValidationEngine)
            : base(originalValue, null, internalObservableValidationEngine)
        {
            _localizationResourceType = localizationResourceType;
            Initialize(originalValue);
        }

        #endregion

        #region Initialization

        private void Initialize(TEnum originalValue)
        {
            // initialize the list of all possible values
            var properties = new List<IEnumItem<TEnum>>();
            var enumMemberValues = (TEnum[])Enum.GetValues(typeof(TEnum));
            foreach (var value in enumMemberValues)
            {
                // fetch the localized string corresponding to the enum value
                var enumMemberName = Enum.GetName(typeof(TEnum), value);
                var resourceID = ResourceUtil.GetResourceId(typeof(TEnum), enumMemberName);

                var uiMetadata = new UIMetadata { LabelProvider = () => ResourceUtil.GetResource(resourceID, _localizationResourceType)};
                var property = new EnumItem<TEnum>(uiMetadata, value);
                properties.Add(property);
            }

            // fills the enum members
            AssignFillItemsSource(() => properties);

            // set the original selected value from the list
            AssignOriginalValue(properties.Select(p => p.Value).First(e => e.Equals(originalValue)));
        }

        #endregion
    }
}
