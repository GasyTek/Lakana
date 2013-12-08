using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GasyTek.Lakana.Navigation.Transitions.Anim2D
{
    /// <summary>
    /// 
    /// </summary>
    public class SlideTransition : Transition
    {
        private const string AnimatedObjectName = "D18FAE8059141A08B3E839B3B712BC6";

        public SlideTransition()
        {
            Duration = new Duration(TimeSpan.FromMilliseconds(500));
        }

        protected override Storyboard CreateAnimation(TransitionInfo transitionInfo)
        {
            return transitionInfo.AnimationType == AnimationType.ShowFrontView
                        ? ShowFrontViewAnimation(transitionInfo)
                        : HideFrontViewAnimation(transitionInfo);
        }

        protected override void OnRunTransitionCompleted(TransitionInfo transitionInfo)
        {
            transitionInfo.SceneNameScope.UnregisterName(AnimatedObjectName);
        }

        #region Private methods

        private Storyboard ShowFrontViewAnimation(TransitionInfo transitionInfo)
        {
            var storyboard = new Storyboard();

            var translateTransform = new TranslateTransform();

            transitionInfo.SceneNameScope.RegisterName(AnimatedObjectName, translateTransform);

            transitionInfo.FrontView.RenderTransform = translateTransform;

            var slideAnimation = new DoubleAnimation
            {
                From = transitionInfo.FrontView.ActualWidth,
                To = 0,
                Duration = Duration,
                EasingFunction = new CubicEase()
            };

            Storyboard.SetTargetName(slideAnimation, AnimatedObjectName);
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath(TranslateTransform.XProperty));

            storyboard.Children.Add(slideAnimation);

            return storyboard;
        }

        private Storyboard HideFrontViewAnimation(TransitionInfo transitionInfo)
        {
            var storyboard = new Storyboard();

            var translateTransform = new TranslateTransform();

            transitionInfo.SceneNameScope.RegisterName(AnimatedObjectName, translateTransform);

            transitionInfo.FrontView.RenderTransform = translateTransform;

            var slideAnimation = new DoubleAnimation
            {
                From = 0,
                To = -1 * transitionInfo.FrontView.ActualWidth,
                Duration = Duration,
                EasingFunction = new CubicEase()
            };

            Storyboard.SetTargetName(slideAnimation, AnimatedObjectName);
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath(TranslateTransform.XProperty));

            storyboard.Children.Add(slideAnimation);

            return storyboard;
        }

        #endregion
    }
}