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

        protected bool IsTransitionning { get; set; }

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

        protected internal Task PerformUIActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            // if an activation is already in progress
            if (IsTransitionning)
            {
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetCanceled();
                return tcs.Task;
            }

            // set that an activation is running
            IsTransitionning = true;

            // deactivates all inputs and event listeners
            Workspace.IsHitTestVisible = false;

            OnBeforeAnimatingActivation(nodeToDeactivate, nodeToActivate);

            ApplyInitialVisibilityBeforeActivation(nodeToDeactivate, nodeToActivate);

            var task = AnimateActivation(nodeToDeactivate, nodeToActivate);
            return task.ContinueWith(r =>
                                         {
                                             ApplyFinalVisibilityAfterActivation(nodeToDeactivate, nodeToActivate);

                                             OnAfterAnimatingActivation(nodeToDeactivate, nodeToActivate);

                                             // activates all inputs and event listeners
                                             Workspace.IsHitTestVisible = true;

                                             // set that an activation no longer runs
                                             IsTransitionning = false;

                                         }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected internal Task PerformUIClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            // if an activation is already in progress
            if (IsTransitionning)
            {
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetCanceled();
                return tcs.Task;
            }

            // set that an activation is running
            IsTransitionning = true;

            // deactivates all inputs and event listeners
            Workspace.IsHitTestVisible = false;

            OnBeforeAnimatingClose(nodeToClose, nodeToActivate);

            ApplyInitialVisibilityBeforeClose(nodeToClose, nodeToActivate);

            var task = AnimateClose(nodeToClose, nodeToActivate);
            return task.ContinueWith(r =>
                                  {
                                      ApplyFinalVisibilityAfterClose(nodeToClose, nodeToActivate);

                                      OnAfterAnimatingClose(nodeToClose, nodeToActivate);

                                      // activates all inputs and event listeners
                                      Workspace.IsHitTestVisible = true;

                                      // set that an activation no longer runs
                                      IsTransitionning = false;

                                  }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #region Private methods

        private Task AnimateActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            var tcs = new TaskCompletionSource<bool>();

            // animates the transition
            if (TransitionAnimationProvider != null)
            {
                var transitionAnimation = TransitionAnimationProvider();
                if (transitionAnimation != null)
                {
                    var viewGroupHostToActivate = nodeToActivate != null
                                                ? OnGetViewGroupHostControl(nodeToActivate.List)
                                                : null;
                    var viewGroupHostToDeactivate = nodeToDeactivate != null
                                                   ? OnGetViewGroupHostControl(nodeToDeactivate.List)
                                                   : null;

                    if (!Equals(viewGroupHostToActivate, viewGroupHostToDeactivate))
                    {
                        // case 1 : if transition from one view group to another
                        if (transitionAnimation.TransitionViewAnimation != null)
                        {
                            return transitionAnimation.TransitionViewGroupAnimation.Run(Workspace,
                                                                                        viewGroupHostToDeactivate,
                                                                                        viewGroupHostToActivate,
                                                                                        AnimationType.ShowFrontView);
                        }
                    }
                    else
                    {
                        // case 2 : if transition from one view to another
                        if (transitionAnimation.TransitionViewAnimation != null)
                        {
                            var activatedView = nodeToActivate != null ? nodeToActivate.Value.ViewHostInstance : null;
                            var deactivatedView = nodeToDeactivate != null ? nodeToDeactivate.Value.ViewHostInstance : null;

                            return transitionAnimation.TransitionViewAnimation.Run(Workspace,
                                                                                    deactivatedView,
                                                                                    activatedView,
                                                                                    AnimationType.ShowFrontView);
                        }
                    }
                }
            }

            tcs.SetCanceled();
            return tcs.Task;
        }

        private Task AnimateClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            var tcs = new TaskCompletionSource<bool>();

            // animates the transition
            if (TransitionAnimationProvider != null)
            {
                var transitionAnimation = TransitionAnimationProvider();
                if (transitionAnimation != null)
                {
                    var viewGroupHostToActivate = nodeToActivate != null
                                                ? OnGetViewGroupHostControl(nodeToActivate.List)
                                                : null;
                    var viewGroupHostToClose = nodeToClose != null
                                                ? OnGetViewGroupHostControl(nodeToClose.List)
                                                : null;

                    if (!Equals(viewGroupHostToClose, viewGroupHostToActivate))
                    {
                        // case 1 : means that we must transition from one view group to another
                        if (transitionAnimation.TransitionViewAnimation != null)
                        {
                            return transitionAnimation.TransitionViewGroupAnimation.Run(Workspace,
                                                                                        viewGroupHostToActivate,
                                                                                        viewGroupHostToClose,
                                                                                        AnimationType.HideFrontView);
                        }
                    }
                    else
                    {
                        // case 2 : means that we can transition from one view to another on the same group
                        if (transitionAnimation.TransitionViewAnimation != null)
                        {
                            var viewHostToActivate = nodeToActivate != null
                                                         ? nodeToActivate.Value.ViewHostInstance
                                                         : null;
                            var viewHostToClose = nodeToClose.Value.ViewHostInstance;

                            return transitionAnimation.TransitionViewAnimation.Run(Workspace,
                                                                                   viewHostToActivate,
                                                                                   viewHostToClose,
                                                                                   AnimationType.HideFrontView);
                        }
                    }
                }
            }

            tcs.SetCanceled();
            return tcs.Task;
        }

        /// <summary>
        /// Applies the correct initial visibility states for all the concerned view before animating them.
        /// </summary>
        private void ApplyInitialVisibilityBeforeActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            var viewHostToDeactivate = nodeToDeactivate != null ? nodeToDeactivate.Value.ViewHostInstance : null;
            var viewHostToActivate = nodeToActivate != null ? nodeToActivate.Value.ViewHostInstance : null;
            var viewGroupHostToDeactivate = nodeToDeactivate != null ? OnGetViewGroupHostControl(nodeToDeactivate.List) : null;
            var viewGroupHostToActivate = nodeToActivate != null ? OnGetViewGroupHostControl(nodeToActivate.List) : null;

            if (Equals(viewGroupHostToDeactivate, viewGroupHostToActivate))
            {
                // if both views are membres of the same view group
                if (viewHostToDeactivate != null) viewHostToDeactivate.Visibility = Visibility.Visible;
                if (viewHostToActivate != null) viewHostToActivate.Visibility = Visibility.Hidden;
                if (viewGroupHostToDeactivate != null) viewGroupHostToDeactivate.Visibility = Visibility.Visible;
                if (viewGroupHostToActivate != null) viewGroupHostToActivate.Visibility = Visibility.Visible;
            }
            else
            {
                // if views are owned by different view group
                if (viewHostToDeactivate != null) viewHostToDeactivate.Visibility = Visibility.Visible;
                if (viewHostToActivate != null) viewHostToActivate.Visibility = Visibility.Visible;
                if (viewGroupHostToDeactivate != null) viewGroupHostToDeactivate.Visibility = Visibility.Visible;
                if (viewGroupHostToActivate != null) viewGroupHostToActivate.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Applies the correct final visibility states for all the concerned view after they were animated.
        /// </summary>
        private void ApplyFinalVisibilityAfterActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            var viewHostToDeactivate = nodeToDeactivate != null ? nodeToDeactivate.Value.ViewHostInstance : null;
            var viewHostToActivate = nodeToActivate != null ? nodeToActivate.Value.ViewHostInstance : null;
            var viewGroupHostToDeactivate = nodeToDeactivate != null ? OnGetViewGroupHostControl(nodeToDeactivate.List) : null;
            var viewGroupHostToActivate = nodeToActivate != null ? OnGetViewGroupHostControl(nodeToActivate.List) : null;

            if (Equals(viewGroupHostToDeactivate, viewGroupHostToActivate))
            {
                // both views are membres of the same view group
                if (viewHostToDeactivate != null) viewHostToDeactivate.Visibility = Visibility.Hidden;
                if (viewHostToActivate != null) viewHostToActivate.Visibility = Visibility.Visible;
                if (viewGroupHostToDeactivate != null) viewGroupHostToDeactivate.Visibility = Visibility.Visible;
                if (viewGroupHostToActivate != null) viewGroupHostToActivate.Visibility = Visibility.Visible;
            }
            else
            {
                if (viewHostToDeactivate != null) viewHostToDeactivate.Visibility = Visibility.Visible;
                if (viewHostToActivate != null) viewHostToActivate.Visibility = Visibility.Visible;
                if (viewGroupHostToDeactivate != null) viewGroupHostToDeactivate.Visibility = Visibility.Hidden;
                if (viewGroupHostToActivate != null) viewGroupHostToActivate.Visibility = Visibility.Visible;
            }
        }

        private void ApplyInitialVisibilityBeforeClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            var viewHostToClose = nodeToClose != null ? nodeToClose.Value.ViewHostInstance : null;
            var viewHostToActivate = nodeToActivate != null ? nodeToActivate.Value.ViewHostInstance : null;
            var viewGroupHostToClose = nodeToClose != null ? OnGetViewGroupHostControl(nodeToClose.List) : null;
            var viewGroupHostToActivate = nodeToActivate != null ? OnGetViewGroupHostControl(nodeToActivate.List) : null;

            if (Equals(viewGroupHostToClose, viewGroupHostToActivate))
            {
                // if both views are membres of the same view group
                if (viewHostToClose != null) viewHostToClose.Visibility = Visibility.Visible;
                if (viewHostToActivate != null) viewHostToActivate.Visibility = Visibility.Visible;
                if (viewGroupHostToClose != null) viewGroupHostToClose.Visibility = Visibility.Visible;
                if (viewGroupHostToActivate != null) viewGroupHostToActivate.Visibility = Visibility.Visible;
            }
            else
            {
                // if views are owned by different view group
                if (viewHostToClose != null) viewHostToClose.Visibility = Visibility.Visible;
                if (viewHostToActivate != null) viewHostToActivate.Visibility = Visibility.Visible;
                if (viewGroupHostToClose != null) viewGroupHostToClose.Visibility = Visibility.Visible;
                if (viewGroupHostToActivate != null) viewGroupHostToActivate.Visibility = Visibility.Hidden;
            }
        }

        private void ApplyFinalVisibilityAfterClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            var viewHostToClose = nodeToClose != null ? nodeToClose.Value.ViewHostInstance : null;
            var viewHostToActivate = nodeToActivate != null ? nodeToActivate.Value.ViewHostInstance : null;
            var viewGroupHostToClose = nodeToClose != null ? OnGetViewGroupHostControl(nodeToClose.List) : null;
            var viewGroupHostToActivate = nodeToActivate != null ? OnGetViewGroupHostControl(nodeToActivate.List) : null;

            if (Equals(viewGroupHostToClose, viewGroupHostToActivate))
            {
                // if both views are membres of the same view group
                if (viewHostToClose != null) viewHostToClose.Visibility = Visibility.Hidden;
                if (viewHostToActivate != null) viewHostToActivate.Visibility = Visibility.Visible;
                if (viewGroupHostToClose != null) viewGroupHostToClose.Visibility = Visibility.Visible;
                if (viewGroupHostToActivate != null) viewGroupHostToActivate.Visibility = Visibility.Visible;
            }
            else
            {
                // if views are owned by different view group
                if (viewHostToClose != null) viewHostToClose.Visibility = Visibility.Visible;
                if (viewHostToActivate != null) viewHostToActivate.Visibility = Visibility.Visible;
                if (viewGroupHostToClose != null) viewGroupHostToClose.Visibility = Visibility.Hidden;
                if (viewGroupHostToActivate != null) viewGroupHostToActivate.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Overridable methods

        /// <summary>
        /// Executed just before the animation that activate a view take place.
        /// </summary>
        /// <param name="nodeToDeactivate"> </param>
        /// <param name="nodeToActivate"> </param>
        protected abstract void OnBeforeAnimatingActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate);

        /// <summary>
        /// Executed just after the animation that activate a view take place.
        /// </summary>
        /// <param name="nodeToDeactivate"></param>
        /// <param name="nodeToActivate"></param>
        protected abstract void OnAfterAnimatingActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate);

        /// <summary>
        /// Executed just before the animation that close a view take place.
        /// </summary>
        /// <param name="nodeToDeactivate"> </param>
        /// <param name="nodeToActivate"> </param>
        protected abstract void OnBeforeAnimatingClose(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate);

        /// <summary>
        /// Executed just after the animation that close a view take place.
        /// </summary>
        /// <param name="nodeToDeactivate"></param>
        /// <param name="nodeToActivate"></param>
        protected abstract void OnAfterAnimatingClose(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate);

        /// <summary>
        /// Retrieves the visual element that is mapped to the given view group.
        /// </summary>
        /// <param name="viewGroup">The view group.</param>
        /// <returns></returns>
        protected abstract ViewGroupHostControl OnGetViewGroupHostControl(ViewGroup viewGroup);

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

        Task IWorkspaceAdapter.PerformUIActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            return PerformUIActivation(nodeToDeactivate, nodeToActivate);
        }

        Task IWorkspaceAdapter.PerformUIClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            return PerformUIClose(nodeToClose, nodeToActivate);
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