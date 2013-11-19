using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Transitions.Anim2D;

namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// Aggregates tha animation objects that will be used to animate transitions between views.
    /// </summary>
    public class TransitionAnimation
    {
        /// <summary>
        /// Gets the animation used when transitioning from one group to another.
        /// </summary>
        /// <value>
        /// The transition view group animation.
        /// </value>
        public Transition TransitionViewGroupAnimation { get; private set; }

        /// <summary>
        /// Gets the animation used when transitioning from one view to another.
        /// </summary>
        /// <value>
        /// The transition view animation.
        /// </value>
        public Transition TransitionViewAnimation { get; private set; }

        private TransitionAnimation(Transition transitionViewGroupAnimation, Transition transitionViewAnimation)
        {
            TransitionViewGroupAnimation = transitionViewGroupAnimation;
            TransitionViewAnimation = transitionViewAnimation;
        }

        #region Factory methods

        public static TransitionAnimation Create(Transition transitionViewGroupAnimation,
                                                 Transition transitionViewAnimation)
        {
            return new TransitionAnimation(transitionViewGroupAnimation, transitionViewAnimation);
        }

        public static TransitionAnimation Create()
        {
            return new TransitionAnimation(new NoTranstion(), new NoTranstion());
        }

        public static TransitionAnimation CreateViewTransition(Transition transitionViewAnimation)
        {
            return new TransitionAnimation(new NoTranstion(), transitionViewAnimation);
        }

        public static TransitionAnimation CreateViewGroupTransition(Transition transitionViewGroupAnimation)
        {
            return new TransitionAnimation(transitionViewGroupAnimation, new NoTranstion());
        }

        #endregion

    }
}