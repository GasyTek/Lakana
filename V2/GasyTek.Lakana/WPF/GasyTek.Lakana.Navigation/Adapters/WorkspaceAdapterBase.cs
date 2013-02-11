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

        public void PerformActivation(LinkedListNode<View> activatedNode, LinkedListNode<View> deactivatedNode)
        {
            OnPerformActivation(activatedNode, deactivatedNode);
            
            // animates the transition
            if (TransitionAnimationProvider != null)
            {
                var transitionAnimation = TransitionAnimationProvider();
                if (transitionAnimation != null)
                {
                    var activatedViewGroup = activatedNode != null
                                                ? OnGetViewGroupMapping((ViewGroup)activatedNode.List)
                                                : null;
                    var deactivatedViewGroup = deactivatedNode != null
                                                   ? OnGetViewGroupMapping((ViewGroup)deactivatedNode.List)
                                                   : null;

                    if (!Equals(activatedViewGroup, deactivatedViewGroup))
                    {
                        // if transition from one view group to another
                        var storyboard = transitionAnimation.TransitionViewGroupAnimation(activatedViewGroup, deactivatedViewGroup);
                        storyboard.Begin();
                    }
                    else
                    {
                        // if transition from one view to another
                        var activatedView = activatedNode != null ? activatedNode.Value.InternalViewInstance : null;
                        var deactivatedView = deactivatedNode != null ? deactivatedNode.Value.InternalViewInstance : null;

                        // if transition from view to another view from the same group
                        var storyboard = transitionAnimation.TransitionViewAnimation(activatedView, deactivatedView);
                        storyboard.Begin();
                    }

                }
            }
        }

        public void PerformClose(LinkedListNode<View> activatedNode, ClosedNode closedNode)
        {
            if (TransitionAnimationProvider != null)
            {

            }

            OnPerformClose(activatedNode, closedNode);
        }

        protected abstract void OnPerformActivation(LinkedListNode<View> activatedNode, LinkedListNode<View> deactivatedNode);
        protected abstract void OnPerformClose(LinkedListNode<View> activatedNode, ClosedNode closedNode);

        protected abstract FrameworkElement OnGetViewGroupMapping(ViewGroup viewGroup);

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