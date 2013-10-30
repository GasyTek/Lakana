using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// A control that mimics the behavior of root windows.
    /// </summary>
    [TemplatePart(Name = "PART_Close", Type = typeof(Button))]
    public class WindowControl : ContentControl
    {
        #region Routed events

        public static readonly RoutedEvent ClosingEvent =
                EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WindowControl));

        #endregion

        #region Dependency properties
        
        public static readonly DependencyProperty CloseCommandProperty =
                DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(WindowControl), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty TitleProperty =
                DependencyProperty.Register("Title", typeof (string), typeof (WindowControl), new PropertyMetadata(default(string)));

        #endregion

        #region Component Keys

        public static ComponentResourceKey CloseButtonStyleKey = new ComponentResourceKey(typeof(WindowControl), "CloseButtonStyle");
        public static ComponentResourceKey TitleStyleKey = new ComponentResourceKey(typeof(WindowControl), "TitleStyle");

        #endregion

        #region Properties

        public string CloseCommand
        {
            get { return (string)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        #endregion

        #region Constructor

        static WindowControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowControl), new FrameworkPropertyMetadata(typeof(WindowControl)));
        }

        #endregion

        #region Overriden methods

        public override void OnApplyTemplate()
        {
            // Close button
            var btnPartClose = GetTemplateChild("PART_Close") as Button;
            if (btnPartClose != null)
            {
                btnPartClose.Click += (sender, args) => RaiseClosingEvent();
            }
        }

        #endregion

        #region Private methods

        private void RaiseClosingEvent()
        {
            var newEventArgs = new RoutedEventArgs(ClosingEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion
    }
}