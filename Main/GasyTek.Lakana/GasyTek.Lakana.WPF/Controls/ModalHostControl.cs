using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GasyTek.Lakana.WPF.Controls
{
    /// <summary>
    /// Control that can hosts a modal view.
    /// </summary>
    public class ModalHostControl : Control
    {
        #region Dependency properties

        public static readonly DependencyProperty ModalContentProperty =
                    DependencyProperty.Register("ModalContent", typeof(object), typeof(ModalHostControl)
                                , new FrameworkPropertyMetadata());

        #endregion

        #region Properties

        public object ModalContent
        {
            get { return GetValue(ModalContentProperty); }
            set { SetValue(ModalContentProperty, value); }
        }

        internal TaskCompletionSource<object> ResultCompletionSource { get; private set; }

        #endregion

        #region Constructor

        static ModalHostControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalHostControl), new FrameworkPropertyMetadata(typeof(ModalHostControl)));
        }

        public ModalHostControl()
        {
            ResultCompletionSource = new TaskCompletionSource<object>();
        }

        #endregion
    }
}
