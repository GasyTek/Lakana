using GasyTek.Lakana.Navigation.Transitions;
using GasyTek.Lakana.Navigation.Transitions.Anim2D;
using GasyTek.Lakana.Navigation.Transitions.Anim3D;

namespace Samples.GasyTek.Lakana.Utils
{
    /// <summary>
    /// A factory that creates different combination of transitions.
    /// </summary>
    public static class TransitionFactory
    {
        public static TransitionAnimation CubeAndSlideTransition()
        {
            return TransitionAnimation.Create(new CubeTransition3D(), new SlideTransition());
        }

        public static TransitionAnimation FlipTransition()
        {
            return TransitionAnimation.Create(
                    new FlipTransition3D { Direction = FlipTransition3D.FlipDirection.RightToLeft},
                    new FlipTransition3D());
            
        }
    }
}
