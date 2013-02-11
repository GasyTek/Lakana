using System.Windows;
using System.Windows.Media.Animation;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Delegate that provides a storyboard to animate the transition between two view groups.
    /// </summary>
    /// <param name="activatedGroup">The activated group.</param>
    /// <param name="deactivatedGroup">The deactivated group.</param>
    /// <returns></returns>
    public delegate Storyboard TransitionViewGroupAnimation(FrameworkElement activatedGroup, FrameworkElement deactivatedGroup);

    /// <summary>
    /// Delegate that provides a storyboard to animate the transition between two views.
    /// </summary>
    /// <param name="activatedView">The activated view.</param>
    /// <param name="deactivatedView">The deactivated view.</param>
    /// <returns></returns>
    public delegate Storyboard TransitionViewAnimation(FrameworkElement activatedView, FrameworkElement deactivatedView);

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
        public TransitionViewGroupAnimation TransitionViewGroupAnimation { get; private set; }

        /// <summary>
        /// Gets the animation used when transitioning from one view to another.
        /// </summary>
        /// <value>
        /// The transition view animation.
        /// </value>
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