using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.WPF.Services;

namespace GasyTek.Lakana.WPF.Controls
{
    /// <summary>
    /// Controls that hosts a message box.
    /// </summary>
    [TemplatePart(Name = "PART_Yes", Type = typeof(Button))]
    [TemplatePart(Name = "PART_No", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Cancel", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Ok", Type = typeof(Button))]
    public class MessageBoxControl : UserControl
    {
        #region Fields

        private const string OkButtonId = "Ok";
        private const string YesButtonId = "Yes";
        private const string NoButtonId = "No";
        private const string CancelButtonId = "Cancel";

        private string _clickedButtonId;
        private readonly Task<MessageBoxResult> _result;

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxControl), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty MessageBoxButtonProperty =
            DependencyProperty.Register("MessageBoxButton", typeof(MessageBoxButton), typeof(MessageBoxControl), new FrameworkPropertyMetadata(MessageBoxButton.OK));

        public static readonly DependencyProperty MessageBoxImageProperty =
            DependencyProperty.Register("MessageBoxImage", typeof(MessageBoxImage), typeof(MessageBoxControl), new FrameworkPropertyMetadata(MessageBoxImage.Information));

        #endregion

        #region Properties

        internal INavigationService NavigationService { get; set; }

        internal string ViewKey { get; set; }

        public Task<MessageBoxResult> Result
        {
            get { return _result; }
        }

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

        public MessageBoxControl()
        {
            _result = new Task<MessageBoxResult>(OnButtonClicked);
        }

        #endregion

        #region Overriden methods

        public override void OnApplyTemplate()
        {
            // Ok button
            var btnPartOk = GetTemplateChild("PART_Ok") as Button;
            if (btnPartOk != null)
            {
                btnPartOk.Click += (sender, args) =>
                                       {
                                           _clickedButtonId = OkButtonId;
                                           _result.RunSynchronously();
                                       };
            }

            // Yes button
            var btnPartYes = GetTemplateChild("PART_Yes") as Button;
            if (btnPartYes != null)
            {
                btnPartYes.Click += (sender, args) =>
                                        {
                                            _clickedButtonId = YesButtonId;
                                            _result.RunSynchronously();
                                        };
            }

            // No button
            var btnPartNo = GetTemplateChild("PART_No") as Button;
            if (btnPartNo != null)
            {
                btnPartNo.Click += (sender, args) =>
                                        {
                                            _clickedButtonId = NoButtonId;
                                            _result.RunSynchronously();
                                        };
            }

            // Cancel button
            var btnPartCancel = GetTemplateChild("PART_Cancel") as Button;
            if (btnPartCancel != null)
            {
                btnPartCancel.Click += (sender, args) =>
                                        {
                                            _clickedButtonId = CancelButtonId;
                                            _result.RunSynchronously();
                                        };
            }
        }

        #endregion

        private MessageBoxResult OnButtonClicked()
        {
            NavigationService.Close(ViewKey);

            // Ok clicked
            if (Equals(_clickedButtonId, OkButtonId))
                return MessageBoxResult.OK;

            // Yes clicked
            if (Equals(_clickedButtonId, YesButtonId))
                return MessageBoxResult.Yes;

            // No clicked
            if (Equals(_clickedButtonId, NoButtonId))
                return MessageBoxResult.No;

            // Cancel clicked
            if (Equals(_clickedButtonId, CancelButtonId))
                return MessageBoxResult.Cancel;

            return MessageBoxResult.None;
        }
    }
}
