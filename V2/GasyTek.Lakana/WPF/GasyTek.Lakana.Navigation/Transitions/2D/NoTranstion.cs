using System.Windows.Media.Animation;

namespace GasyTek.Lakana.Navigation.Transitions.Anim2D
{
    /// <summary>
    /// 
    /// </summary>
    public class NoTranstion : Transition
    {
        protected override Storyboard CreateAnimation(TransitionInfo transitionInfo)
        {
            return new Storyboard();
        }
    }
}