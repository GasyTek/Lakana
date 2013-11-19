using System;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Services;
using GasyTek.Lakana.Navigation.Transitions;

namespace GasyTek.Lakana.Navigation.Adapters
{
    /// <summary>
    /// Base class for workspace adapters
    /// </summary>
    /// <typeparam name="TWorkspace">The type of the panel.</typeparam>
    public abstract class WorkspaceAdapterBase<TWorkspace> : IWorkspaceAdapter where TWorkspace : Panel
    {
        public TWorkspace Workspace { get; private set; }
        public ViewGroupCollection ViewGroupCollection { get; private set; }
        public Func<TransitionAnimation> TransitionAnimationProvider { get; private set; }

        protected internal void SetMainWorkspace(TWorkspace workspace)
        {
            Workspace = workspace;
        }

        protected internal void SetViewGroupCollection(ViewGroupCollection viewGroupCollection)
        {
            ViewGroupCollection = viewGroupCollection;
        }

        protected internal void SetTransitionAnimationProvider(Func<TransitionAnimation> transitionAnimationProvider)
        {
            TransitionAnimationProvider = transitionAnimationProvider;
        }

        protected internal void PerformActivation(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode)
        {
            OnPerformActivation(activatedNode, deactivatedNode);

            // animates the transition
            if (TransitionAnimationProvider != null)
            {
                var transitionAnimation = TransitionAnimationProvider();
                if (transitionAnimation != null)
                {
                    var activatedViewGroup = activatedNode != null
                                                ? OnGetViewGroupMapping(activatedNode.List)
                                                : null;
                    var deactivatedViewGroup = deactivatedNode != null
                                                   ? OnGetViewGroupMapping(deactivatedNode.List)
                                                   : null;

                    if (!Equals(activatedViewGroup, deactivatedViewGroup))
                    {
                        // if transition from one view group to another
                        //var transition = transitionAnimation.TransitionViewGroupAnimation(activatedViewGroup, deactivatedViewGroup);
                        //transition.Run(Workspace);
                        //storyboard.Begin();
                    }
                    else
                    {
                        // if transition from one view to another
                        var activatedView = activatedNode != null ? activatedNode.Value.InternalViewInstance : null;
                        var deactivatedView = deactivatedNode != null ? deactivatedNode.Value.InternalViewInstance : null;

                        // if transition from view to another view from the same group
                        if( transitionAnimation.TransitionViewAnimation != null)
                        {
                            transitionAnimation.TransitionViewAnimation.Run(Workspace, deactivatedView, activatedView);
                        }
                    }
                }
            }
        }

        protected internal void PerformClose(ViewGroupNode activatedNode, ViewGroupNode closedNode)
        {
            if (TransitionAnimationProvider != null)
            {

            }

            OnPerformClose(activatedNode, closedNode);
        }

        #region Overridable methods

        /// <summary>
        /// Perform the actual activation of a view.
        /// </summary>
        /// <param name="activatedNode"></param>
        /// <param name="deactivatedNode"></param>
        protected abstract void OnPerformActivation(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode);

        /// <summary>
        /// Perform the actual closing of a view.
        /// </summary>
        /// <param name="activatedNode">The new activated view.</param>
        /// <param name="closedNode">The closed view.</param>
        protected abstract void OnPerformClose(ViewGroupNode activatedNode, ViewGroupNode closedNode);

        /// <summary>
        /// Retrieves the visual element that is mapped to the given view group.
        /// </summary>
        /// <param name="viewGroup">The view group.</param>
        /// <returns></returns>
        protected abstract ViewGroupHostControl OnGetViewGroupMapping(ViewGroup viewGroup);

        #endregion

        #region IWorkspaceAdapter members

        void IWorkspaceAdapter.SetMainWorkspace(Panel workspace)
        {
            SetMainWorkspace((TWorkspace)workspace);
        }

        void IWorkspaceAdapter.SetViewGroupCollection(ViewGroupCollection viewGroupCollection)
        {
            SetViewGroupCollection(viewGroupCollection);
        }

        void IWorkspaceAdapter.SetTransitionAnimationProvider(Func<TransitionAnimation> transitionAnimationProvider)
        {
            SetTransitionAnimationProvider(transitionAnimationProvider);
        }

        void IWorkspaceAdapter.PerformActivation(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode)
        {
            PerformActivation(activatedNode, deactivatedNode);
        }

        void IWorkspaceAdapter.PerformClose(ViewGroupNode activatedNode, ViewGroupNode closedNode)
        {
            PerformClose(activatedNode, closedNode);
        }

        #endregion

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