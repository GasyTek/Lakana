using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.WPF.Services;

namespace GasyTek.Lakana.WPF.Controls
{
    /// <summary>
    /// Custom control that serve to display a list of all tasks that necessitates a user action to be terminated.
    /// </summary>
    [TemplatePart(Name = "PART_Cancel", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ExitApplication", Type = typeof(Button))]
    public class CloseApplicationControl : ItemsControl
    {
        #region Properties

        internal INavigationService NavigationService { get; set; }
        internal string ViewKey { get; set; }

        public int NbCloseableViews
        {
            get { return (Items != null) ? Items.Count : 0; }
        }

        #endregion

        #region Constructor

        static CloseApplicationControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseApplicationControl), new FrameworkPropertyMetadata(typeof(CloseApplicationControl)));
        }
        
        #endregion

        #region Overriden methods

        public override void OnApplyTemplate()
        {
            // Cancel button
            var btnPartCancel = GetTemplateChild("PART_Cancel") as Button;
            if (btnPartCancel != null)
            {
                btnPartCancel.Click += (sender, args) =>
                {
                    if(NavigationService != null && !string.IsNullOrEmpty(ViewKey))
                    {
                        NavigationService.Close(ViewKey);
                    }
                };
            }

            // ExitApplication button
            var btnPartExitApplication = GetTemplateChild("PART_ExitApplication") as Button;
            if (btnPartExitApplication != null)
            {
                btnPartExitApplication.Click += (sender, args) =>
                {
                    if (NavigationService != null && !string.IsNullOrEmpty(ViewKey))
                    {
                        NavigationService.CloseApplication(true);
                    }
                };
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CloseApplicationControlItem {NavigationService = NavigationService, OwnerViewKey = ViewKey};
        }

        #endregion
    }
}
