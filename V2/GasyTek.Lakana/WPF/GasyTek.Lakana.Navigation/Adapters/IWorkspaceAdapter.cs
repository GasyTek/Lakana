using System;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;
using GasyTek.Lakana.Navigation.Transitions;

namespace GasyTek.Lakana.Navigation.Adapters
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWorkspaceAdapter
    {
        /// <summary>
        /// Sets the main workspace.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        void SetMainWorkspace(FrameworkElement workspace);
        /// <summary>
        /// Sets the view stack collection.
        /// </summary>
        /// <param name="viewGroupCollection">The view stack collection.</param>
        void SetViewGroupCollection(ViewGroupCollection viewGroupCollection);
        /// <summary>
        /// Sets the transition animation provider.
        /// </summary>
        /// <param name="transitionAnimationProvider">The transition animation provider.</param>
        void SetTransitionAnimationProvider(Func<TransitionAnimation> transitionAnimationProvider);
        /// <summary>
        /// Performs activation and deactivation of the new and old views.
        /// </summary>
        /// <param name="activatedNode">The activated node.</param>
        /// <param name="deactivatedNode">The deactivated node.</param>
        void PerformActivation(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode);

        /// <summary>
        /// Performs the close.
        /// </summary>
        /// <param name="activatedNode">The activated node.</param>
        /// <param name="closedNode">The closed node.</param>
        void PerformClose(ViewGroupNode activatedNode, ViewGroupNode closedNode);
    }
}
