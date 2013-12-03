using System.Windows.Controls;
using System.Windows.Markup;
using GasyTek.Lakana.Navigation.Controls;

namespace GasyTek.Lakana.Navigation.Transitions
{
    /// <summary>
    /// Used to share the transition animation necessary infos.
    /// </summary>
    public class TransitionInfo
    {
        public Panel Scene { get; set; }
        public INameScope SceneNameScope { get; set; }
        public HostControl BackView { get; set; }
        public HostControl FrontView { get; set; }
        public AnimationType AnimationType { get; set; }

        public double SceneWidth
        {
            get { return Scene != null ? Scene.ActualWidth : 0; }
        }
    }
}