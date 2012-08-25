using System.Windows;

namespace GasyTek.Lakana.WPF.Services
{
    /// <summary>
    /// Provides consumers the ability to animate the transitioning of the current view and new views.
    /// </summary>
    /// <param name="currentView">Currently displayed FrameworkElement</param>
    /// <param name="newView">New FrameworkElement to be displayed</param>
    public delegate void AnimateTransitionAction(FrameworkElement currentView, FrameworkElement newView);
}