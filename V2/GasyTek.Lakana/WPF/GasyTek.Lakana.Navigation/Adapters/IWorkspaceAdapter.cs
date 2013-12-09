using System;
using System.Threading.Tasks;
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
        /// Performs asynchronous activation and deactivation of the new and old views.
        /// </summary>
        /// <param name="nodeToDeactivate">The node being deactivated.</param>
        /// <param name="nodeToActivate">The node being activated.</param>
        Task PerformUIActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate);

        /// <summary>
        /// Performs asynchronous close.
        /// </summary>
        /// <param name="nodeToClose">The closed node.</param>
        /// <param name="nodeToActivate">The activated node.</param>
        Task PerformUIClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate);
    }
}
