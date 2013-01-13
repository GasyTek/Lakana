using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    internal class GridWorkspaceAdapter : WorkspaceAdapterBase<Grid>
    {
        public override void PerformActivation(LinkedListNode<View> activatedNode, LinkedListNode<View> deactivatedNode)
        {
            if (deactivatedNode != null)
            {
                foreach (var viewInfo in deactivatedNode.List)
                {
                    viewInfo.InternalViewInstance.Visibility = Visibility.Hidden;
                }
            }

            if (activatedNode != null)
            {
                var zIndex = 10;
                var activatedView = activatedNode.Value.InternalViewInstance;
                activatedView.Visibility = Visibility.Visible;
                activatedView.IsEnabled = true;
                Panel.SetZIndex(activatedView, zIndex);

                if (!Workspace.Children.Contains(activatedView))
                    Workspace.Children.Add(activatedView);

                if (activatedNode.Value.IsModal)
                {
                    zIndex = 0;
                    foreach (var viewInfo in activatedNode.List)
                    {
                        viewInfo.InternalViewInstance.Visibility = Visibility.Visible;
                        viewInfo.InternalViewInstance.IsEnabled = Equals(viewInfo.InternalViewInstance, activatedView);
                        Panel.SetZIndex(viewInfo.InternalViewInstance, zIndex);
                        zIndex++;
                    }
                }
            }

            if (TransitionAnimationProvider != null)
            {
                var transitionAnimation = TransitionAnimationProvider();
                if (transitionAnimation != null)
                {
                    var activatedViewGroup = activatedNode != null 
                                                ? (ViewGroup) activatedNode.List 
                                                : new ViewGroup();
                    var deactivatedViewGroup = deactivatedNode != null
                                                   ? (ViewGroup) deactivatedNode.List
                                                   : new ViewGroup();

                    if (activatedViewGroup != deactivatedViewGroup)
                    {
                        // if transition from view group to another
                        var storyboard = transitionAnimation.TransitionViewGroupAnimation(activatedViewGroup, deactivatedViewGroup);
                        storyboard.Begin();
                    }
                    else
                    {
                        var activatedView = activatedNode != null ? activatedNode.Value.InternalViewInstance : null;
                        var deactivatedView = deactivatedNode != null ? deactivatedNode.Value.InternalViewInstance : null;

                        // if transition from view to another view from the same group
                        var storyboard = transitionAnimation.TransitionViewAnimation(activatedView, deactivatedView);
                        storyboard.Begin();
                    }

                }
            }
        }

        public override void PerformClose(LinkedListNode<View> activatedNode, LinkedListNode<View> closedNode)
        {
            if (closedNode != null)
            {
                Workspace.Children.Remove(closedNode.Value.InternalViewInstance);
            }

            if (activatedNode != null)
            {
                PerformActivation(activatedNode, null);
            }

        }
    }
}
