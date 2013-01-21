using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    public interface IWorkspaceAdapter
    {
        /// <summary>
        /// Sets the main workspace.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        void SetMainWorkspace(Panel workspace);
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
        void PerformActivation(LinkedListNode<View> activatedNode, LinkedListNode<View> deactivatedNode);

        /// <summary>
        /// Performs the close.
        /// </summary>
        /// <param name="activatedNode">The activated node.</param>
        /// <param name="closedNode">The closed node.</param>
        void PerformClose(LinkedListNode<View> activatedNode, ClosedNode closedNode);
    }
}
