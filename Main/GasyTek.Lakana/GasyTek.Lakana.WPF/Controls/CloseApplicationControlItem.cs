using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.WPF.Services;

namespace GasyTek.Lakana.WPF.Controls
{
    /// <summary>
    /// Represents an item for <see cref="CloseApplicationControl" />
    /// </summary>
    public class CloseApplicationControlItem : Button
    {
        #region Dependency properties

        public static readonly DependencyProperty ViewKeyProperty =
                        DependencyProperty.Register("ViewKey", typeof(string), typeof(CloseApplicationControlItem), new PropertyMetadata(default(string)));

        #endregion

        #region Properties

        internal INavigationService NavigationService { get; set; }
        internal string OwnerViewKey { get; set; }

        public string ViewKey
        {
            get { return (string)GetValue(ViewKeyProperty); }
            set { SetValue(ViewKeyProperty, value); }
        }

        #endregion

        #region Constructor

        static CloseApplicationControlItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseApplicationControlItem), new FrameworkPropertyMetadata(typeof(CloseApplicationControlItem)));
        }

        public CloseApplicationControlItem()
        {
            Click += (sender, args) =>
                              {
                                  if (NavigationService == null) return;
                                  if (string.IsNullOrEmpty(OwnerViewKey)) return;
                                  if (string.IsNullOrEmpty(ViewKey)) return;
                                  NavigationService.Close(OwnerViewKey);
                                  NavigationService.NavigateTo(ViewKey);
                              };
        }

        #endregion
    }
}