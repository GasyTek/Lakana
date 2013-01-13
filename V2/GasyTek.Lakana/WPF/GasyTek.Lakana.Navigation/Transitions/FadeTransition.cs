using System.Linq;
using System.Windows;
using System;
using System.Windows.Media.Animation;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// Contains default transition animations that users can leverage.
    /// </summary>
    public static class FadeTransition
    {
        public static TransitionAnimation Create()
        {
            return new TransitionAnimation(ViewGroupAnimation, ViewAnimation);
        }

        private static Storyboard ViewGroupAnimation(ViewGroup activatedGroup, ViewGroup deactivatedGroup)
        {
            const double animationDuration = 200;

            var activatedStack = activatedGroup.ToStack();
            var deactivatedStack = deactivatedGroup.ToStack();
            var transitionAnimation = new Storyboard();
            var deactivatedView = deactivatedStack.Any() ? deactivatedStack.Peek() : null;
            var activatedView = activatedStack.Any() ? activatedStack.Peek() : null;

            // if one tries to deactivate then reactivate the same view, do nothing
            if (Equals(deactivatedView, activatedView)) return transitionAnimation;

            if (deactivatedView != null)
            {
                foreach (var view in deactivatedStack)
                {
                    // current view animation
                    var opacityAnimation = new DoubleAnimation
                                                            {
                                                                From = 1,
                                                                To = 0,
                                                                Duration = TimeSpan.FromMilliseconds(animationDuration),
                                                                FillBehavior = FillBehavior.Stop,
                                                                AccelerationRatio = 0.5
                                                            };

                    Storyboard.SetTarget(opacityAnimation, view);
                    Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                    transitionAnimation.Children.Add(opacityAnimation);
                }
            }

            if (activatedView != null)
            {
                foreach (var view in activatedStack)
                {
                    // new view animation
                    var opacityAnimation = new DoubleAnimation
                                                            {
                                                                From = 0.5,
                                                                To = 1,
                                                                Duration = TimeSpan.FromMilliseconds(animationDuration)
                                                            };

                    Storyboard.SetTarget(opacityAnimation, view);
                    Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                    transitionAnimation.Children.Add(opacityAnimation);
                }
            }

            return transitionAnimation;
        }

        private static Storyboard ViewAnimation(FrameworkElement activatedView, FrameworkElement deactivatedView)
        {
            const double animationDuration = 200;

            var transitionAnimation = new Storyboard();

            // if one tries to deactivate then reactivate the same view, do nothing
            if (Equals(deactivatedView, activatedView)) return transitionAnimation;

            if (activatedView != null)
            {
                // new view animation
                var opacityAnimation = new DoubleAnimation
                {
                    From = 0.5,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(animationDuration)
                };

                Storyboard.SetTarget(opacityAnimation, activatedView);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                transitionAnimation.Children.Add(opacityAnimation);
            }

            return transitionAnimation;
        }
    }

    public static class ViewTransitions
    {

        
    }
}
