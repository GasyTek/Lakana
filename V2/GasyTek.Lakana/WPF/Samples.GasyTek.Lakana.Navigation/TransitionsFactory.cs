using GasyTek.Lakana.Navigation.Transitions;
using GasyTek.Lakana.Navigation.Transitions.Anim2D;

namespace Samples.GasyTek.Lakana.Navigation
{
    /// <summary>
    /// A factory that creates different combination of transitions.
    /// </summary>
    public static class TransitionsFactory
    {
        public static  TransitionAnimation NoTransition()
        {
            return TransitionAnimation.Create();
        }

        public static TransitionAnimation Transition1()
        {
            return TransitionAnimation.CreateViewTransition(new SlideTransition());
        }
    }
}