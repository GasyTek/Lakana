using GasyTek.Lakana.Navigation.Transitions;
using GasyTek.Lakana.Navigation.Transitions.Anim2D;

namespace Samples.GasyTek.Lakana.Utils
{
    /// <summary>
    /// A factory that creates different combination of transitions.
    /// </summary>
    public static class TransitionFactory
    {
        public static TransitionAnimation CubeAndFadeTransition()
        {
            return TransitionAnimation.Create(new SlideTransition(), new FadeTransition());
        }
    }
}
