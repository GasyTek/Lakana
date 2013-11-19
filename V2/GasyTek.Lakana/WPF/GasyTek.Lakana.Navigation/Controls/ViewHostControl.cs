﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// Host a view to support animation during transition.
    /// </summary>
    public class ViewHostControl : FrameworkElement
    {
        #region Fields

        private UIElement _view;

        #endregion

        #region Properties

        public UIElement View
        {
            get { return _view; }
            set
            {
                if (value is Window)
                    throw new ViewTypeNotSupportedByWorkspaceAdapterException(value.GetType());

                _view = value;
                AddVisualChild(_view);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Takes a snapshot of the appearance of the control.
        /// </summary>
        /// <returns></returns>
        public Brush TakeSnapshot()
        {
            // Ensures the visual is visible
            var previousVisibility = Visibility;
            Visibility = Visibility.Visible;

            // Create the bitmap that will contain the snapshot
            var bmp = new RenderTargetBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);

            // Creates then fill the drawing to render as bitmap
            var drawingVisual = new DrawingVisual();
            using (var dc = drawingVisual.RenderOpen())
            {
                var vb = new VisualBrush(this);
                dc.DrawRectangle(vb, null, new Rect(new Point(), RenderSize));
            }

            // Renders the wpf visual to bitmap
            bmp.Render(drawingVisual);

            Visibility = previousVisibility;

            return new ImageBrush(bmp);
        }

        public void ClearProperties()
        {
            // cf. http://msdn.microsoft.com/en-us/library/ms749010(v=vs.90).aspx
            var locallySetProperties = GetLocalValueEnumerator();
            while (locallySetProperties.MoveNext())
            {
                var propertyToClear = locallySetProperties.Current.Property;
                if (!propertyToClear.ReadOnly) { ClearValue(propertyToClear); }
            }
        }

        #endregion

        #region Overriden methods

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return View;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            View.Measure(constraint);
            return View.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            View.Arrange(new Rect(finalSize));
            return View.RenderSize;
        }

        #endregion
    }
}