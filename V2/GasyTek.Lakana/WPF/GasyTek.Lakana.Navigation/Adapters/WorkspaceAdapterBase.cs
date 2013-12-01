using System;
using System.Threading.Tasks;
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

        protected bool IsPerformingActivation { get; set; }

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

        protected internal Task PerformActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            // if an activation is already running
            if (IsPerformingActivation)
            {
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetCanceled();
                return tcs.Task;
            }

            // set that an activation is running
            IsPerformingActivation = true;

            // deactivates all inputs and event listeners
            Workspace.IsHitTestVisible = false;


            OnBeforePerformTransition(nodeToActivate, nodeToDeactivate);

            var task = PerformTransitionAnimation(nodeToActivate, nodeToDeactivate);
            return task.ContinueWith(r =>
                {
                    OnAfterPerformTransition(nodeToActivate, nodeToDeactivate);

                    // activates all inputs and event listeners
                    Workspace.IsHitTestVisible = true;

                    // set that an activation no longer runs
                    IsPerformingActivation = false;

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected internal Task PerformClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            if (TransitionAnimationProvider != null)
            {

            }

            return OnPerformClose(nodeToClose, nodeToActivate);
        }

        private Task PerformTransitionAnimation(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode)
        {
            var tcs = new TaskCompletionSource<bool>();

            // animates the transition
            if (TransitionAnimationProvider != null)
            {
                var transitionAnimation = TransitionAnimationProvider();
                if (transitionAnimation != null)
                {
                    var activatedViewGroupHost = activatedNode != null
                                                ? OnGetViewGroupMapping(activatedNode.List)
                                                : null;
                    var deactivatedViewGroupHost = deactivatedNode != null
                                                   ? OnGetViewGroupMapping(deactivatedNode.List)
                                                   : null;

                    if (!Equals(activatedViewGroupHost, deactivatedViewGroupHost))
                    {
                        // case 1 : if transition from one view group to another
                        if (transitionAnimation.TransitionViewAnimation != null)
                        {
                            return transitionAnimation.TransitionViewGroupAnimation.Run(Workspace, deactivatedViewGroupHost, activatedViewGroupHost, AnimationType.ShowFrontView);
                        }
                    }
                    else
                    {
                        // case 2 : if transition from one view to another
                        var activatedView = activatedNode != null ? activatedNode.Value.InternalViewInstance : null;
                        var deactivatedView = deactivatedNode != null ? deactivatedNode.Value.InternalViewInstance : null;

                        // if transition from view to another view from the same group
                        if (transitionAnimation.TransitionViewAnimation != null)
                        {
                            return transitionAnimation.TransitionViewAnimation.Run(Workspace, deactivatedView, activatedView, AnimationType.ShowFrontView);
                        }
                    }
                }
            }

            tcs.SetCanceled();
            return tcs.Task;
        }

        #region Overridable methods

        /// <summary>
        /// Executed just before the animation of views transition take place.
        /// </summary>
        /// <param name="activatedNode"></param>
        /// <param name="deactivatedNode"></param>
        protected abstract void OnBeforePerformTransition(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode);

        /// <summary>
        /// Executed just after the animation of views transition take place.
        /// </summary>
        /// <param name="activatedNode"></param>
        /// <param name="deactivatedNode"></param>
        protected abstract void OnAfterPerformTransition(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode);

        /// <summary>
        /// Perform the actual closing of a view.
        /// </summary>
        /// <param name="nodeToClose">The closed view.</param>
        /// <param name="nodeToActivate">The new activated view.</param>
        protected abstract Task OnPerformClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate);

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

        Task IWorkspaceAdapter.PerformActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            return PerformActivation(nodeToDeactivate, nodeToActivate);
        }

        Task IWorkspaceAdapter.PerformClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            return PerformClose(nodeToClose, nodeToActivate);
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