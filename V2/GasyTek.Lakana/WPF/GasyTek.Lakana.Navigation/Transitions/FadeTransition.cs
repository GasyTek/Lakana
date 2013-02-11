using System.Windows;
using System;
using System.Windows.Media.Animation;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// A provider for a Fade animation.
    /// </summary>
    public static class FadeTransition
    {
        const double AnimationDuration = 180;

        public static TransitionAnimation Create()
        {
            return new TransitionAnimation(ViewGroupAnimation, ViewAnimation);
        }

        private static Storyboard ViewGroupAnimation(FrameworkElement activatedGroup, FrameworkElement deactivatedGroup)
        {
            var transitionAnimation = new Storyboard();

            // if one tries to deactivate then reactivate the same view group then do nothing
            if (Equals(activatedGroup, deactivatedGroup)) return transitionAnimation;

            if (deactivatedGroup != null)
            {
                // current view animation
                var opacityAnimation = new DoubleAnimation
                                                        {
                                                            From = 1,
                                                            To = 0,
                                                            Duration = TimeSpan.FromMilliseconds(AnimationDuration),
                                                            FillBehavior = FillBehavior.Stop,
                                                            AccelerationRatio = 0.5
                                                        };

                Storyboard.SetTarget(opacityAnimation, deactivatedGroup);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                transitionAnimation.Children.Add(opacityAnimation);
            }

            if (activatedGroup != null)
            {
                // new view animation
                var opacityAnimation = new DoubleAnimation
                                                        {
                                                            From = 0.5,
                                                            To = 1,
                                                            Duration = TimeSpan.FromMilliseconds(AnimationDuration)
                                                        };

                Storyboard.SetTarget(opacityAnimation, activatedGroup);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                transitionAnimation.Children.Add(opacityAnimation);
            }

            return transitionAnimation;
        }

        private static Storyboard ViewAnimation(FrameworkElement activatedView, FrameworkElement deactivatedView)
        {
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
                    Duration = TimeSpan.FromMilliseconds(AnimationDuration)
                };

                Storyboard.SetTarget(opacityAnimation, activatedView);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                transitionAnimation.Children.Add(opacityAnimation);
            }

            return transitionAnimation;
        }
    }
}
