using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// Base class for classes used to host views.
    /// </summary>
    public abstract class HostControl : FrameworkElement
    {
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
    }
}
