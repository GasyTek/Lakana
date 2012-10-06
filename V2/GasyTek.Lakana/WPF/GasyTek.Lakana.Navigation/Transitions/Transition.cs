using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// Contains default transition animations that users can leverage.
    /// </summary>
    /// <remarks>Original source : "Ocean Framework" by Karl Shifflett : http://karlshifflett.wordpress.com/ocean/ </remarks>
    public static class Transition
    {
        /// <summary>
        /// A transition without animations
        /// </summary>
        /// <param name="currentView">The current view.</param>
        /// <param name="newView">The new view.</param>
        public static void NoTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            // nothing to do in fact
        }

        /// <summary>
        /// Fades the transition.
        /// </summary>
        /// <param name="currentView">The obj current view.</param>
        /// <param name="newView">The obj new view.</param>
        public static void FadeTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            const double animationDuration = 100;

            if(currentView == newView) return;

            if (currentView == null)
            {
                newView.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(animationDuration))));
            }
            else
            {
                var currentViewAnimation = new DoubleAnimation
                                               {
                                                   From = 1,
                                                   To = 0,
                                                   Duration = TimeSpan.FromMilliseconds(animationDuration),
                                                   FillBehavior = FillBehavior.Stop,
                                                   AccelerationRatio = 0.5
                                               };

                var newViewAnimation = new DoubleAnimation
                                                {
                                                    From = 0,
                                                    To = 1,
                                                    Duration = TimeSpan.FromMilliseconds(animationDuration),
                                                    AccelerationRatio = 0.5
                                                };

                currentView.BeginAnimation(UIElement.OpacityProperty, currentViewAnimation);
                newView.BeginAnimation(UIElement.OpacityProperty, newViewAnimation);
            }
        }

        public static void ZoomTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            const double animationDuration = 300;

            if (newView != null)
            {
                var scaleTransform = new ScaleTransform { ScaleX = 0, ScaleY = 0 };
                newView.RenderTransform = scaleTransform;
                newView.RenderTransformOrigin = new Point(0.5, 0.5);

                var xAnimation = new DoubleAnimation
                                    {
                                        From = 0,
                                        To = 1,
                                        Duration = TimeSpan.FromMilliseconds(animationDuration),
                                        AccelerationRatio = 0.5
                                    };
                var yAnimation = new DoubleAnimation
                                    {
                                        From = 0,
                                        To = 1,
                                        Duration = TimeSpan.FromMilliseconds(animationDuration),
                                        AccelerationRatio = 0.5
                                    };

                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, xAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, yAnimation);
            }
        }
    }
}
