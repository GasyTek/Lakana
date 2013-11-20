using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// Host a group of view to support animation during transition.
    /// </summary>
    [ContentProperty("Views")]
    public class ViewGroupHostControl : HostControl
    {
        public UIElementCollection Views { get; private set; }

        public ViewGroupHostControl()
        {
            Views = new UIElementCollection(this, this);
        }

        #region Overriden methods

        protected override Visual GetVisualChild(int index)
        {
            return Views[index];
        }

        protected override int VisualChildrenCount
        {
            get { return Views.Count; }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double width = 0d, height = 0d;

            foreach (UIElement view in Views)
            {
                view.Measure(availableSize);

                if (view.DesiredSize.Width > width) width = view.DesiredSize.Width;
                if (view.DesiredSize.Height > height) height = view.DesiredSize.Height;
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement view in Views)
            {
                view.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }

        #endregion
    }
}