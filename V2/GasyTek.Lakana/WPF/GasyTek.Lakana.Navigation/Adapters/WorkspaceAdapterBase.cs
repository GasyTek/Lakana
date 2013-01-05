using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    /// <summary>
    /// Base class for workspace adapters
    /// </summary>
    /// <typeparam name="TPanel">The type of the panel.</typeparam>
    public abstract class WorkspaceAdapterBase<TPanel> : IWorkspaceAdapter where TPanel : Panel
    {
        public TPanel Workspace { get; private set; }
        public ViewGroupCollection ViewGroupCollection { get; private set; }
        public Func<TransitionAnimation> TransitionAnimationProvider { get; private set; }

        public void SetMainWorkspace(Panel workspace)
        {
            Workspace = (TPanel)workspace;
        }

        public void SetViewGroupCollection(ViewGroupCollection viewGroupCollection)
        {
            ViewGroupCollection = viewGroupCollection;
        }

        public void SetTransitionAnimationProvider(Func<TransitionAnimation> transitionAnimationProvider)
        {
            TransitionAnimationProvider = transitionAnimationProvider;
        }

        public abstract void PerformActivation(LinkedListNode<View> activatedNode, LinkedListNode<View> deactivatedNode);
        public abstract void PerformClose(LinkedListNode<View> activatedNode, LinkedListNode<View> closedNode);

        #region IActiveAware support

        /// <summary>
        /// Support for active aware view and view models <see cref="IActiveAware"/>
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="viewModel">The view model.</param>
        protected void EnforceOnActivating(FrameworkElement view, object viewModel)
        {
            // Notifies activation on the view 
            var activeAwareView = view as IActiveAware;
            if (activeAwareView != null) activeAwareView.OnActivating();

            // Notifies activation on the view model
            var activeAwareViewModel = viewModel as IActiveAware;
            if (activeAwareViewModel != null) activeAwareViewModel.OnActivating();
        }

        /// <summary>
        /// Support for active aware view and view models <see cref="IActiveAware"/>
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="viewModel">The view model.</param>
        protected void EnforceOnActivated(FrameworkElement view, object viewModel)
        {
            // Notifies activation on the view 
            var activeAwareView = view as IActiveAware;
            if (activeAwareView != null) activeAwareView.OnActivated();

            // Notifies activation on the view model
            var activeAwareViewModel = viewModel as IActiveAware;
            if (activeAwareViewModel != null) activeAwareViewModel.OnActivated();
        }

        /// <summary>
        /// Support for active aware view and view models <see cref="IActiveAware"/>
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="viewModel">The view model.</param>
        protected void EnforceOnDeactivating(FrameworkElement view, object viewModel)
        {
            // Notifies deactivation on the view 
            var activeAwareView = view as IActiveAware;
            if (activeAwareView != null) activeAwareView.OnDeactivating();

            // Notifies deactivation on the view model
            var activeAwareViewModel = viewModel as IActiveAware;
            if (activeAwareViewModel != null) activeAwareViewModel.OnDeactivating();
        }

        /// <summary>
        /// Support for active aware view and view models <see cref="IActiveAware"/>
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="viewModel">The view model.</param>
        protected void EnforceOnDeactivated(FrameworkElement view, object viewModel)
        {
            // Notifies deactivation on the view 
            var activeAwareView = view as IActiveAware;
            if (activeAwareView != null) activeAwareView.OnDeactivated();

            // Notifies deactivation on the view model
            var activeAwareViewModel = viewModel as IActiveAware;
            if (activeAwareViewModel != null) activeAwareViewModel.OnDeactivated();
        }

        #endregion
    }
}