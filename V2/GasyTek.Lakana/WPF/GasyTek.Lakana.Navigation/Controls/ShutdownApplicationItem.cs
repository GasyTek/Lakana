using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// Represents an item for <see cref="ShutdownApplicationWindow" />
    /// </summary>
    public class ShutdownApplicationItem : Button
    {
        #region Attached events

        public static readonly RoutedEvent TargetViewSelectedEvent =
            EventManager.RegisterRoutedEvent("TargetViewSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ShutdownApplicationItem));

        public static void AddTargetViewSelectedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            var uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(TargetViewSelectedEvent, handler);
            }
        }
        public static void RemoveTargetViewSelectedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            var uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(TargetViewSelectedEvent, handler);
            }
        }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty TargetViewInstanceKeyProperty =
            DependencyProperty.Register("TargetViewInstanceKey", typeof(string), typeof(ShutdownApplicationItem), new PropertyMetadata(default(string)));

        #endregion

        #region Properties

        internal INavigationManager NavigationManager { get; set; }

        public string TargetViewInstanceKey
        {
            get { return (string)GetValue(TargetViewInstanceKeyProperty); }
            set { SetValue(TargetViewInstanceKeyProperty, value); }
        }

        #endregion

        #region Constructor

        static ShutdownApplicationItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShutdownApplicationItem), new FrameworkPropertyMetadata(typeof(ShutdownApplicationItem)));
        }

        public ShutdownApplicationItem()
        {
            Click += (sender, args) =>
                              {
                                  if (NavigationManager == null) return;
                                  if (string.IsNullOrEmpty(TargetViewInstanceKey)) return;
                                  RaiseEvent(new RoutedEventArgs(TargetViewSelectedEvent));
                                  NavigationManager.NavigateTo(TargetViewInstanceKey);
                              };
        }

        #endregion
    }
}