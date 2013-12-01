using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Controls
{
    /// <summary>
    /// Host a view to support animation during transition.
    /// </summary>
    public class ViewHostControl : HostControl
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
                if(value == null)
                    throw new InvalidOperationException("View can't be set to null");

                if(value.GetType() == typeof(Window))
                    throw new ViewTypeNotSupportedByWorkspaceAdapterException(value.GetType());

                _view = value;
                AddLogicalChild(value);
                AddVisualChild(value);
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
