using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace GasyTek.Lakana.Navigation.Services
{
    public delegate Storyboard TransitionViewGroupAnimation(ViewGroup activateGroup, ViewGroup deactivateGroup);
    public delegate Storyboard TransitionViewAnimation(FrameworkElement activatedView, FrameworkElement deactivatedView);

    public class TransitionAnimation
    {
        public TransitionViewGroupAnimation TransitionViewGroupAnimation { get; private set; }
        public TransitionViewAnimation TransitionViewAnimation { get; private set; }

        public TransitionAnimation(TransitionViewGroupAnimation transitionViewGroupAnimation, TransitionViewAnimation transitionViewAnimation)
        {
            TransitionViewGroupAnimation = transitionViewGroupAnimation;
            TransitionViewAnimation = transitionViewAnimation;
        }

        public TransitionAnimation(TransitionViewGroupAnimation transitionViewGroupAnimation)
            : this(transitionViewGroupAnimation, (activatedView, deactivatedView) => new Storyboard())
        {
        }

        public TransitionAnimation(TransitionViewAnimation transitionViewAnimation)
            : this((activateGroup, deactivateGroup) => new Storyboard(), transitionViewAnimation)
        {
        }

        public TransitionAnimation()
            : this((activateGroup, deactivateGroup) => new Storyboard(), (activatedView, deactivatedView) => new Storyboard())
        {
        }
    }
}