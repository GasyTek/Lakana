using System.Windows;
using System.Windows.Controls;

namespace GasyTek.Lakana.WPF.Controls
{
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

        #endregion

        #region Constructor

        static ModalHostControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalHostControl), new FrameworkPropertyMetadata(typeof(ModalHostControl)));
        }

        #endregion
    }
}
