using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GasyTek.Lakana.WPF.Controls
{
    /// <summary>
    /// Control that can hosts a modal view.
    /// </summary>
    public abstract class ModalHostControl : Control
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

        public void SetModalResult(object result)
        {
            OnSetModalResult(result);
        }

        protected abstract void OnSetModalResult(object result);
    }

    public class ModalHostControl<TResult> : ModalHostControl
    {
        internal TaskCompletionSource<TResult> ResultCompletionSource { get; private set; }

        public ModalHostControl()
        {
            ResultCompletionSource = new TaskCompletionSource<TResult>();
        }

        protected override void OnSetModalResult(object result)
        {
            if (result == null)
                ResultCompletionSource.SetResult(default(TResult));
            else
                ResultCompletionSource.SetResult((TResult) result);
        }
    }
}
