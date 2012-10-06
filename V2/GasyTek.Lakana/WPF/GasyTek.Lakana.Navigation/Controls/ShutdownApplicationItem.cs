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

        public static readonly DependencyProperty TargetViewKeyProperty =
            DependencyProperty.Register("TargetViewKey", typeof(string), typeof(ShutdownApplicationItem), new PropertyMetadata(default(string)));

        #endregion

        #region Properties

        internal INavigationService NavigationService { get; set; }

        public string TargetViewKey
        {
            get { return (string)GetValue(TargetViewKeyProperty); }
            set { SetValue(TargetViewKeyProperty, value); }
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
                                  if (NavigationService == null) return;
                                  if (string.IsNullOrEmpty(TargetViewKey)) return;
                                  RaiseEvent(new RoutedEventArgs(TargetViewSelectedEvent));
                                  NavigationService.NavigateTo(TargetViewKey);
                              };
        }

        #endregion
    }
}