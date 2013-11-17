using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace GasyTek.Lakana.Navigation.Transitions.Anim2D
{
    public class FadeTransition : Transition
    {
        public FadeTransition()
        {
            Duration = new Duration(TimeSpan.FromSeconds(1));
        }

        protected override Storyboard CreateAnimation(TransitionInfo transitionInfo)
        {
            var storyboard = new Storyboard();

            var opacityAnimation = new DoubleAnimation
                                       {
                                           From = 1d,
                                           To = 0d,
                                           Duration = Duration
                                       };

            Storyboard.SetTarget(opacityAnimation, transitionInfo.OldItem);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

            storyboard.Children.Add(opacityAnimation);

            return storyboard;
        }
    }
}