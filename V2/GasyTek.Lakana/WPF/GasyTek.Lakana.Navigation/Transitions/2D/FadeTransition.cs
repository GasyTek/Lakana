using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace GasyTek.Lakana.Navigation.Transitions.Anim2D
{
    /// <summary>
    /// 
    /// </summary>
    public class FadeTransition : Transition
    {
        public FadeTransition()
        {
            Duration = new Duration(TimeSpan.FromSeconds(1));
        }

        protected override Storyboard CreateAnimation(TransitionInfo transitionInfo)
        {
            return transitionInfo.AnimationType == AnimationType.ShowFrontView
                       ? ShowFrontViewAnimation(transitionInfo)
                       : HideFrontViewAnimation(transitionInfo);
        }

        #region Private methods

        private Storyboard ShowFrontViewAnimation(TransitionInfo transitionInfo)
        {
            var storyboard = new Storyboard();

            var opacityAnimation = new DoubleAnimation
            {
                From = 0d,
                To = 1d,
                Duration = Duration
            };

            Storyboard.SetTarget(opacityAnimation, transitionInfo.FrontView);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }

        private Storyboard HideFrontViewAnimation(TransitionInfo transitionInfo)
        {
            var storyboard = new Storyboard();

            var opacityAnimation = new DoubleAnimation
            {
                From = 1d,
                To = 0d,
                Duration = Duration
            };

            Storyboard.SetTarget(opacityAnimation, transitionInfo.FrontView);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }

        #endregion
    }
}