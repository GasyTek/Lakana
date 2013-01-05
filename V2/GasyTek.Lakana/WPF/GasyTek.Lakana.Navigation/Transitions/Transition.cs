using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// Contains default transition animations that users can leverage.
    /// </summary>
    public static class Transition
    {
        /// <summary>
        /// A transition without animations
        /// </summary>
        /// <param name="activatedGroup">Group of nodes to activate.</param>
        /// <param name="deactivatedGroup">Group of nodes to deactivat.</param>
        /// <remarks>The views at the top of each stack are the current and new view.</remarks>
        public static Storyboard NoTransition(Stack<FrameworkElement> activatedGroup, Stack<FrameworkElement> deactivatedGroup)
        {
            return null;
        }

        /// <summary>
        /// Fades the transition.
        /// </summary>
        /// <param name="activatedGroup">Group of nodes to activate.</param>
        /// <param name="deactivatedGroup">Group of nodes to deactivat.</param>
        /// <remarks>The views at the top of each stack are the current and new view.</remarks>
        public static Storyboard FadeTransition(ViewGroup activatedGroup, ViewGroup deactivatedGroup)
        {
            const double animationDuration = 200;

            var activatedStack = activatedGroup.ToStack();
            var deactivatedStack = deactivatedGroup.ToStack();
            var transitionAnimation = new Storyboard();
            var deactivatedView = deactivatedStack.Any() ? deactivatedStack.Peek() : null;
            var activatedView = activatedStack.Any() ? activatedStack.Peek() : null;

            // if one tries to deactivate then reactivate the same view, do not nothing
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
