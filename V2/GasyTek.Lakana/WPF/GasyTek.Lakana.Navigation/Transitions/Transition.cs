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
        public static Storyboard NoTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            return null;
        }

        /// <summary>
        /// Fades the transition.
        /// </summary>
        /// <param name="currentView">The obj current view.</param>
        /// <param name="newView">The obj new view.</param>
        public static Storyboard FadeTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            const double animationDuration = 200;

            if (Equals(currentView, newView)) return null;

            Storyboard transitionAnimation;

            if (currentView == null)
            {
                // new view animation
                var newViewOpacityAnimation = new DoubleAnimation(0.5, 1,
                                                           new Duration(TimeSpan.FromMilliseconds(animationDuration)));
                Storyboard.SetTarget(newViewOpacityAnimation, newView);
                Storyboard.SetTargetProperty(newViewOpacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                transitionAnimation = new Storyboard();
                transitionAnimation.Children.Add(newViewOpacityAnimation);
            }
            else
            {
                // current view animation
                var currentViewOpacityAnimation = new DoubleAnimation
                                                    {
                                                        From = 1,
                                                        To = 0,
                                                        Duration = TimeSpan.FromMilliseconds(animationDuration),
                                                        FillBehavior = FillBehavior.Stop,
                                                        AccelerationRatio = 0.5
                                                    };
                Storyboard.SetTarget(currentViewOpacityAnimation, currentView);
                Storyboard.SetTargetProperty(currentViewOpacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                // new view animation
                var newViewOpacityAnimation = new DoubleAnimation
                                                    {
                                                        From = 0.5,
                                                        To = 1,
                                                        Duration = TimeSpan.FromMilliseconds(animationDuration),
                                                        AccelerationRatio = 0.5
                                                    };
                Storyboard.SetTarget(newViewOpacityAnimation, newView);
                Storyboard.SetTargetProperty(newViewOpacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                transitionAnimation = new Storyboard();
                transitionAnimation.Children.Add(currentViewOpacityAnimation);
                transitionAnimation.Children.Add(newViewOpacityAnimation);
            }

            return transitionAnimation;
        }

        public static Storyboard ZoomTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            const double animationDuration = 300;


            var scaleTransform = new ScaleTransform { ScaleX = 1, ScaleY = 1 };
            newView.RenderTransform = scaleTransform;
            newView.RenderTransformOrigin = new Point(0.5, 0.5);

            var xAnimation = new DoubleAnimation
                                {
                                    From = 0,
                                    To = 1,
                                    Duration = TimeSpan.FromMilliseconds(animationDuration),
                                    AccelerationRatio = 0.5
                                };
            Storyboard.SetTarget(xAnimation, newView);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("RenderTransform.ScaleX"));

            var yAnimation = new DoubleAnimation
                                {
                                    From = 0,
                                    To = 1,
                                    Duration = TimeSpan.FromMilliseconds(animationDuration),
                                    AccelerationRatio = 0.5
                                };
            Storyboard.SetTarget(yAnimation, newView);
            Storyboard.SetTargetProperty(yAnimation, new PropertyPath("RenderTransform.ScaleY"));

            var transitionAnimation = new Storyboard();
            transitionAnimation.Children.Add(xAnimation);
            transitionAnimation.Children.Add(yAnimation);

            // hide current view
            currentView.Visibility = Visibility.Collapsed;

            return transitionAnimation;
        }
    }
}
