using GasyTek.Lakana.Common.Communication;
using System.Globalization;
using System.Threading;

namespace GasyTek.Lakana.Common.UI
{
    /// <summary>
    /// 
    /// </summary>
    public static class LocalizationManager
    {
        /// <summary>
        /// Changes the current culture of the application.
        /// </summary>
        /// <param name="cultureInfo">The new culture info.</param>
        public static void ChangeCulture(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;

            // Notify the rest of the world that localization settings has changed
            MessageBus.Publish(new CultureSettingsChangedEvent(null, cultureInfo));
        }
    }

    /// <summary>
    /// Global event that notifies the rest of the world that the localization settings have changed.
    /// </summary>
    public class CultureSettingsChangedEvent : Message
    {
        /// <summary>
        /// Gets the new culture.
        /// </summary>
        /// <value>
        /// The new culture.
        /// </value>
        public CultureInfo NewCulture
        {
            get; 
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureSettingsChangedEvent" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="newCulture">The new culture.</param>
        public CultureSettingsChangedEvent(object sender, CultureInfo newCulture)
            : base(sender)
        {
            NewCulture = newCulture;
        }
    }
}
