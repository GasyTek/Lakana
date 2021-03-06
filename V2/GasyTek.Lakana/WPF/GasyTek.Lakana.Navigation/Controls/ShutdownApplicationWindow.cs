using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Controls
{
    [TemplatePart(Name = "PART_Cancel", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ExitApplication", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Views", Type = typeof(ShutdownApplicationItemsControl))]
    public class ShutdownApplicationWindow : Window
    {
        #region Dependency properties

        public static readonly DependencyProperty ViewsProperty =
                    DependencyProperty.Register("Views", typeof(ObservableCollection<View>), typeof(ShutdownApplicationWindow), new UIPropertyMetadata());

        #endregion

        #region Component Keys

        public static ComponentResourceKey CancelButtonStyleKey = new ComponentResourceKey(typeof(ShutdownApplicationWindow), "CancelButtonStyle");
        public static ComponentResourceKey ExitButtonStyleKey = new ComponentResourceKey(typeof(ShutdownApplicationWindow), "ExitButtonStyle");

        public static ComponentResourceKey ShutdownApplicationItemsControlStyleKey = new ComponentResourceKey(typeof(ShutdownApplicationWindow), "ShutdownApplicationItemsControlStyle");
        public static ComponentResourceKey ShutdownApplicationItemStyleKey = new ComponentResourceKey(typeof(ShutdownApplicationWindow), "ShutdownApplicationItemStyle");
        
        #endregion

        #region Properties

        internal INavigationManager NavigationManager { get; set; }

        public ObservableCollection<View> Views
        {
            get { return (ObservableCollection<View>)GetValue(ViewsProperty); }
            set { SetValue(ViewsProperty, value); }
        }

        #endregion

        #region Constructor

        static ShutdownApplicationWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShutdownApplicationWindow), new FrameworkPropertyMetadata(typeof(ShutdownApplicationWindow)));
        }

        public ShutdownApplicationWindow()
        {
            ShutdownApplicationItem.AddTargetViewSelectedHandler(this, (sender, args) => DialogResult = false);
        }

        #endregion

        #region Overriden methods

        public override void OnApplyTemplate()
        {
            // Cancel button
            var btnPartCancel = GetTemplateChild("PART_Cancel") as Button;
            if (btnPartCancel != null)
            {
                btnPartCancel.Click += (sender, args) => DialogResult = false;
            }

            // ExitApplication button
            var btnPartExitApplication = GetTemplateChild("PART_ExitApplication") as Button;
            if (btnPartExitApplication != null)
            {
                btnPartExitApplication.Click += (sender, args) =>
                {
                    if (NavigationManager != null)
                    {
                        DialogResult = true;
                        NavigationManager.CloseApplication(true);
                    }
                };
            }

            // List of closeable views
            var shutdownItemsControl = GetTemplateChild("PART_Views") as ShutdownApplicationItemsControl;
            if(shutdownItemsControl != null)
            {
                shutdownItemsControl.NavigationManager = NavigationManager;
            }
        }

        #endregion
    }
}