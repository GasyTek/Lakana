using System;
using System.Windows.Media;
using GasyTek.Lakana.Common.Base;
using GasyTek.Lakana.Common.Extensions;
using GasyTek.Lakana.Common.Utils;

namespace GasyTek.Lakana.Common.UI
{
    /// <summary>
    /// <see cref="IUIMetadata"/>
    /// </summary>
    public class UIMetadata : NotifyPropertyChangedBase, IUIMetadata
    {
        private Func<string> _labelProvider;
        private Func<string> _descriptionProvider;
        private Func<ImageSource> _iconProvider;

        #region Properties

        public string Label
        {
            get { return (_labelProvider != null) ? _labelProvider() : GlobalConstants.LocalizationNoText; }
        }

        public Func<string> LabelProvider
        {
            get { return _labelProvider; }
            set
            {
                _labelProvider = value;
                this.NotifyPropertyChanged(o => o.Label);
            }
        }

        public string Description
        {
            get { return (_descriptionProvider != null) ? _descriptionProvider() : GlobalConstants.LocalizationNoText; }
        }

        public Func<string> DescriptionProvider
        {
            get { return _descriptionProvider; }
            set
            {
                _descriptionProvider = value;
                this.NotifyPropertyChanged(o => o.Description);
            }
        }

        public ImageSource Icon
        {
            get { return (_iconProvider != null) ? _iconProvider() : null; }
        }

        public Func<ImageSource> IconProvider
        {
            get { return _iconProvider; }
            set
            {
                _iconProvider = value;
                this.NotifyPropertyChanged(o => o.Icon);
            }
        }

        #endregion
    }
}