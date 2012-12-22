using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// Controls that hosts a message box.
    /// </summary>
    [TemplatePart(Name = "PART_Yes", Type = typeof(Button))]
    [TemplatePart(Name = "PART_No", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Cancel", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Ok", Type = typeof(Button))]
    public class MessageBoxControl : ContentControl
    {
        #region Dependency properties

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxControl), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty MessageBoxButtonProperty =
            DependencyProperty.Register("MessageBoxButton", typeof(MessageBoxButton), typeof(MessageBoxControl), new FrameworkPropertyMetadata(MessageBoxButton.OK));

        public static readonly DependencyProperty MessageBoxImageProperty =
            DependencyProperty.Register("MessageBoxImage", typeof(MessageBoxImage), typeof(MessageBoxControl), new FrameworkPropertyMetadata(MessageBoxImage.Information));
        
        #endregion

        #region Component Keys

        public static ComponentResourceKey YesButtonStyleKey = new ComponentResourceKey(typeof(MessageBoxControl), "YesButtonStyle");
        public static ComponentResourceKey NoButtonStyleKey = new ComponentResourceKey(typeof(MessageBoxControl), "NoButtonStyle");
        public static ComponentResourceKey CancelButtonStyleKey = new ComponentResourceKey(typeof(MessageBoxControl), "CancelButtonStyle");
        public static ComponentResourceKey OkButtonStyleKey = new ComponentResourceKey(typeof(MessageBoxControl), "OkButtonStyle");

        public static ComponentResourceKey InfoIconContentTemplateKey = new ComponentResourceKey(typeof(MessageBoxControl), "InfoIconContentTemplate");
        public static ComponentResourceKey QuestionIconContentTemplateKey = new ComponentResourceKey(typeof(MessageBoxControl), "QuestionIconContentTemplate");
        public static ComponentResourceKey ErrorIconContentTemplateKey = new ComponentResourceKey(typeof(MessageBoxControl), "ErrorIconContentTemplate");
        public static ComponentResourceKey WarningIconContentTemplateKey = new ComponentResourceKey(typeof(MessageBoxControl), "WarningIconContentTemplate");

        #endregion

        #region Properties

        internal INavigationManager NavigationManager { get; set; }

        internal string ViewInstanceKey { get; set; }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public MessageBoxButton MessageBoxButton
        {
            get { return (MessageBoxButton)GetValue(MessageBoxButtonProperty); }
            set { SetValue(MessageBoxButtonProperty, value); }
        }

        public MessageBoxImage MessageBoxImage
        {
            get { return (MessageBoxImage)GetValue(MessageBoxImageProperty); }
            set { SetValue(MessageBoxImageProperty, value); }
        }


        #endregion

        #region Constructor

        static MessageBoxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxControl), new FrameworkPropertyMetadata(typeof(MessageBoxControl)));
        }

        #endregion

        #region Overriden methods

        public override void OnApplyTemplate()
        {
            // Ok button
            var btnPartOk = GetTemplateChild("PART_Ok") as Button;
            if (btnPartOk != null)
            {
                btnPartOk.Click += (sender, args) => NavigationManager.Close(ViewInstanceKey, MessageBoxResult.OK);
            }

            // Yes button
            var btnPartYes = GetTemplateChild("PART_Yes") as Button;
            if (btnPartYes != null)
            {
                btnPartYes.Click += (sender, args) => NavigationManager.Close(ViewInstanceKey, MessageBoxResult.Yes);
            }

            // No button
            var btnPartNo = GetTemplateChild("PART_No") as Button;
            if (btnPartNo != null)
            {
                btnPartNo.Click += (sender, args) => NavigationManager.Close(ViewInstanceKey, MessageBoxResult.No);
            }

            // Cancel button
            var btnPartCancel = GetTemplateChild("PART_Cancel") as Button;
            if (btnPartCancel != null)
            {
                btnPartCancel.Click += (sender, args) => NavigationManager.Close(ViewInstanceKey, MessageBoxResult.Cancel);
            }
        }

        #endregion
    }
}
