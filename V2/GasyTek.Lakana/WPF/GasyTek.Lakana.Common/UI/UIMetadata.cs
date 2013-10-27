using System;
using System.Windows.Media;
using GasyTek.Lakana.Common.Base;
using GasyTek.Lakana.Common.Extensions;
using GasyTek.Lakana.Common.Utils;
using GasyTek.Lakana.Common.Communication;

namespace GasyTek.Lakana.Common.UI
{
    /// <summary>
    /// <see cref="IUIMetadata"/>
    /// </summary>
    public class UIMetadata : NotifyPropertyChangedBase, IUIMetadata, IMessageListener<CultureSettingsChangedEvent>
    {
        private Func<string> _labelProvider;
        private Func<string> _descriptionProvider;
        private Func<ImageSource> _iconProvider;

        #region Properties

        /// <summary>
        /// Gets the label.
        /// </summary>
        public string Label
        {
            get { return (_labelProvider != null) ? _labelProvider() : GlobalConstants.LocalizationNoText; }
        }

        /// <summary>
        /// Gets or sets the label provider.
        /// </summary>
        /// <value>
        /// The label provider.
        /// </value>
        public Func<string> LabelProvider
        {
            get { return _labelProvider; }
            set
            {
                _labelProvider = value;
                this.NotifyPropertyChanged(o => o.Label);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get { return (_descriptionProvider != null) ? _descriptionProvider() : GlobalConstants.LocalizationNoText; }
        }

        /// <summary>
        /// Gets or sets the description provider.
        /// </summary>
        /// <value>
        /// The description provider.
        /// </value>
        public Func<string> DescriptionProvider
        {
            get { return _descriptionProvider; }
            set
            {
                _descriptionProvider = value;
                this.NotifyPropertyChanged(o => o.Description);
            }
        }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        public ImageSource Icon
        {
            get { return (_iconProvider != null) ? _iconProvider() : null; }
        }

        /// <summary>
        /// Gets or sets the icon provider.
        /// </summary>
        /// <value>
        /// The icon provider.
        /// </value>
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

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UIMetadata" /> class.
        /// </summary>
        public UIMetadata()
        {
            // Subscribes to the culture settings changed event
            MessageBus.Subscribe(this);
        }

        #endregion

        #region IMessageListener

        /// <summary>
        /// Gets or sets the subscription token.
        /// </summary>
        /// <value>
        /// The subscription token.
        /// </value>
        public ISubscriptionToken<CultureSettingsChangedEvent> SubscriptionToken { get; set; }

        /// <summary>
        /// Called when [message received].
        /// </summary>
        /// <param name="message">The message.</param>
        public void OnMessageReceived(CultureSettingsChangedEvent message)
        {
            this.NotifyPropertyChanged(o => o.Label);
            this.NotifyPropertyChanged(o => o.Description);
            this.NotifyPropertyChanged(o => o.Icon);
        }

        #endregion

    }
}