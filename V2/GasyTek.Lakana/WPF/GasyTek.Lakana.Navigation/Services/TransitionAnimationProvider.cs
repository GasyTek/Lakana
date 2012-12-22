using System.Windows;
using System.Windows.Media.Animation;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Provides consumers the ability to animate the transitioning of the current view and new views.
    /// </summary>
    /// <param name="currentView">Currently displayed FrameworkElement</param>
    /// <param name="newView">New FrameworkElement to be displayed</param>
    public delegate Storyboard TransitionAnimationProvider(FrameworkElement currentView, FrameworkElement newView);
}