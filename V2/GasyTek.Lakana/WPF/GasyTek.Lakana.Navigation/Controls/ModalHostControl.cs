using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// Control that can hosts a modal view.
    /// </summary>
    [TemplatePart(Name = "PART_Panel", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_Draggable", Type = typeof(FrameworkElement))]
    public class ModalHostControl : Control
    {
        #region Dependency properties

        public static readonly DependencyProperty ModalContentProperty =
                    DependencyProperty.Register("ModalContent", typeof(object), typeof(ModalHostControl)
                                , new FrameworkPropertyMetadata());

        #endregion

        #region Component Keys

        public static ComponentResourceKey BackgroundBrushKey = new ComponentResourceKey(typeof(MessageBoxControl), "BackgroundBrush");

        #endregion

        #region Properties

        private Canvas ParentPanel { get; set; }
        private FrameworkElement DraggableElement { get; set; }

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

        #region Private methods

        private void ResetDraggableElementPosition()
        {
            var w1 = DraggableElement.ActualWidth / 2.0;
            var w2 = ParentPanel.ActualWidth / 2.0;
            var h1 = DraggableElement.ActualHeight / 2.0;
            var h2 = ParentPanel.ActualHeight / 2.0;
            Canvas.SetTop(DraggableElement, h2 - h1);
            Canvas.SetLeft(DraggableElement, w2 - w1);
        }

        private void InitializeDraggingOperation()
        {
            // hook to mouse events to execute the drag operation 
            var isDragging = false;
            var offset = new Point(0,0);
            var initialPos = new Point(0, 0);

            DraggableElement.MouseDown += (sender, args) =>
            {
                if (DraggableElement.IsMouseCaptureWithin == false)
                {
                    DraggableElement.CaptureMouse();
                    offset = args.GetPosition(DraggableElement);
                    initialPos = args.GetPosition(null);
                    isDragging = true;
                }
            };

            DraggableElement.MouseMove += (sender, args) =>
            {
                if (isDragging && DraggableElement.IsMouseCaptured)
                {
                    var currentPos = args.GetPosition(null);
                    if (Math.Abs(currentPos.X - initialPos.X) >= SystemParameters.MinimumHorizontalDragDistance
                        || Math.Abs(currentPos.Y - initialPos.Y) >= SystemParameters.MinimumVerticalDragDistance)
                    {
                        var newPos = args.GetPosition(ParentPanel);
                        var left = newPos.X - offset.X;
                        var top = newPos.Y - offset.Y;

                        double computedLeft;
                        double computeTop;

                        ComputePosition(left, top, out computedLeft, out computeTop);

                        Canvas.SetTop(DraggableElement, computeTop);
                        Canvas.SetLeft(DraggableElement, computedLeft);
                    }
                }
            };

            DraggableElement.MouseUp += (sender, args) =>
            {
                isDragging = false;

                if (DraggableElement.IsMouseCaptured)
                    DraggableElement.ReleaseMouseCapture();
            };
        }

        private void ComputePosition(double inLeft, double inTop, out double outLeft, out double outTop)
        {
            // compute position so that the draggable element can't go outside of the allowed bounds

            outLeft = inLeft;
            outTop = inTop;

            if (inLeft <= 0) outLeft = 0;
            if (inLeft >= (ParentPanel.ActualWidth - DraggableElement.ActualWidth)) 
                outLeft = (ParentPanel.ActualWidth - DraggableElement.ActualWidth);
            if (inTop <= 0) outTop = 0;
            if (inTop >= (ParentPanel.ActualHeight - DraggableElement.ActualHeight))
                outTop = (ParentPanel.ActualHeight - DraggableElement.ActualHeight);
        }

        #endregion

        #region Overriden methods

        public override void OnApplyTemplate()
        {
            ParentPanel = GetTemplateChild("PART_Panel") as Canvas;
            DraggableElement = GetTemplateChild("PART_Draggable") as FrameworkElement;

            if (ParentPanel == null) throw new InvalidOperationException("PART_Panel have to be of type Canvas");
            if (DraggableElement == null) throw new InvalidOperationException("PART_Draggable have to be of type FrameworkElement");

            // recenter the draggable element when any size changed
            ParentPanel.SizeChanged += (sender, args) => ResetDraggableElementPosition();
            DraggableElement.SizeChanged += (sender, args) => ResetDraggableElementPosition();

            // initialize the dragging operation
            InitializeDraggingOperation();
        }

        #endregion
    }
}
