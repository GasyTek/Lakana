using GasyTek.Lakana.Navigation.Transitions;
using GasyTek.Lakana.Navigation.Transitions.Anim2D;
using GasyTek.Lakana.Navigation.Transitions.Anim3D;

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

        public static TransitionAnimation SlideTransition()
        {
            return TransitionAnimation.CreateViewTransition(new SlideTransition());
        }

        public static TransitionAnimation FadeTransition()
        {
            return TransitionAnimation.CreateViewTransition(new FadeTransition());
        }

        public static  TransitionAnimation Cube3DAnimation()
        {
            return TransitionAnimation.CreateViewGroupTransition(new Cube3DTransition());
        }
    }
}