using System.Windows;
using System.Windows.Media;
using System;
using System.Windows.Media.Animation;

namespace GasyTek.Lakana.WPF.Transitions
{
    /// <summary>
    /// Contains default transition animations that users can leverage.
    /// </summary>
    public static class Transitions
    {
        /// <summary>
        /// A transition without animations
        /// </summary>
        /// <param name="currentView">The current view.</param>
        /// <param name="newView">The new view.</param>
        public static void NoTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            if (currentView != null)
            {
                currentView.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// A single panel come from left to right and it contains the new view.
        /// </summary>
        /// <param name="currentView">The obj current view.</param>
        /// <param name="newView">The obj new view.</param>
        public static void SinglePaneRightToLeftTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            const double ANIMATION_DURATION = 400;

            var newViewtranslateTransform = newView.RenderTransform as TranslateTransform;

            if (newViewtranslateTransform == null)
            {
                newViewtranslateTransform = new TranslateTransform();
                newView.RenderTransformOrigin = new Point(0.5, 0.5);
                newView.RenderTransform = newViewtranslateTransform;
            }

            newViewtranslateTransform.BeginAnimation(TranslateTransform.XProperty
                , new DoubleAnimation(newView.ActualWidth + 20, 0
                , new Duration(TimeSpan.FromMilliseconds(ANIMATION_DURATION))) { DecelerationRatio = 1 });

            if (currentView != null)
            {
                currentView.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Doubles the pane right to left transition.
        /// </summary>
        /// <param name="container">The obj container.</param>
        /// <param name="currentView">The current view.</param>
        /// <param name="newView">The new view.</param>
        public static void DoublePaneRightToLeftTransition(FrameworkElement container, FrameworkElement currentView, FrameworkElement newView)
        {
            if (currentView == null)
            {
                SinglePaneRightToLeftTransition(null, newView);
                return;
            }

            const double ANIMATION_DURATION = 400;
            const string NEW_VIEW_TRANSLATE_TRANSFORM = "NEW_VIEW_TRANSLATE_TRANSFORM";
            const string CURRENT_VIEW_TRANSLATE_TRANSFORM = "CURRENT_VIEW_TRANSLATE_TRANSFORM";

            var storyboard = new Storyboard();
            NameScope.SetNameScope(container, new NameScope());

            // animate the current view

            var currentViewTranslateTransform = currentView.RenderTransform as TranslateTransform;

            if (currentViewTranslateTransform == null)
            {
                currentViewTranslateTransform = new TranslateTransform();
                currentView.RenderTransformOrigin = new Point(0.5, 0.5);
                currentView.RenderTransform = currentViewTranslateTransform;
            }

            container.RegisterName(CURRENT_VIEW_TRANSLATE_TRANSFORM, currentViewTranslateTransform);

            var daNewView = new DoubleAnimationUsingKeyFrames {BeginTime = TimeSpan.FromSeconds(0)};

            Storyboard.SetTargetName(daNewView, NEW_VIEW_TRANSLATE_TRANSFORM);
            Storyboard.SetTargetProperty(daNewView, new PropertyPath("X"));
            daNewView.KeyFrames.Add(new SplineDoubleKeyFrame(newView.ActualWidth + 10, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
            daNewView.KeyFrames.Add(new SplineDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(ANIMATION_DURATION))));
            storyboard.Children.Add(daNewView);

            // animate the new view
            
            var newViewTranslateTransform = newView.RenderTransform as TranslateTransform;

            if (newViewTranslateTransform == null)
            {
                newViewTranslateTransform = new TranslateTransform();
                newView.RenderTransformOrigin = new Point(0.5, 0.5);
                newView.RenderTransform = newViewTranslateTransform;
            }

            container.RegisterName(NEW_VIEW_TRANSLATE_TRANSFORM, newViewTranslateTransform);

            var daCurrentView = new DoubleAnimationUsingKeyFrames
                                    {BeginTime = TimeSpan.FromSeconds(0), FillBehavior = FillBehavior.Stop};
            Storyboard.SetTargetName(daCurrentView, CURRENT_VIEW_TRANSLATE_TRANSFORM);
            Storyboard.SetTargetProperty(daCurrentView, new PropertyPath("X"));
            daCurrentView.KeyFrames.Add(new SplineDoubleKeyFrame((newView.ActualWidth + 10) * -1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(ANIMATION_DURATION))));
            storyboard.Children.Add(daCurrentView);
            
            // begin the animation
            storyboard.Begin(container);
        }

        /// <summary>
        /// Fades the transition.
        /// </summary>
        /// <param name="currentView">The obj current view.</param>
        /// <param name="newView">The obj new view.</param>
        public static void FadeTransition(FrameworkElement currentView, FrameworkElement newView)
        {
            const double ANIMATION_DURATION = 400;

            if (currentView == null)
            {
                newView.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(ANIMATION_DURATION))));
            }
            else
            {
                currentView.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(ANIMATION_DURATION)), FillBehavior.Stop));
                newView.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(ANIMATION_DURATION))));
            }
        }
    }
}
