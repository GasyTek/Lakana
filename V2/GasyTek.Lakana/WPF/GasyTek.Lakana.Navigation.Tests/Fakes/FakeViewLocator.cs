using System;
using System.Windows;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Tests.Fakes
{
    class FakeViewLocator : IViewLocator
    {
        public void RegisterApplicationViews()
        {
        }

        public FrameworkElement GetViewInstance(string viewKey)
        {
            if(viewKey == "view1")
                return new FrameworkElement();

            if (viewKey == "view2")
                return new FrameworkElement();

            if (viewKey == "view3")
                return new FrameworkElement();

            if (viewKey == "parentView1")
                return new FrameworkElement();

            if (viewKey == "parentView2")
                return new FrameworkElement();

            return null;
        }
    }
}
