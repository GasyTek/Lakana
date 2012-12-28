using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    internal class GridWorkspaceAdapter : IWorkspaceAdapter
    {
        private Grid _workspace;

        public void SetMainWorkspace(Panel workspace)
        {
            _workspace = (Grid)workspace;
        }

        public void SetViewStackCollection(ViewStackCollection viewStackCollection)
        {
        }

        public void PerformActivation(LinkedListNode<ViewInfo> activatedNode, LinkedListNode<ViewInfo> deactivatedNode)
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

                if (!_workspace.Children.Contains(activatedView))
                    _workspace.Children.Add(activatedView);

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

            //var aView = activatedNode != null ? activatedNode.Value.InternalViewInstance : null;
            //var dView = deactivatedNode != null ? deactivatedNode.Value.InternalViewInstance : null;

            //var storyboard = Transitions.Transition.FadeTransition(dView, aView);
            //storyboard.Begin();
        }

        public void PerformClose(LinkedListNode<ViewInfo> activatedNode, LinkedListNode<ViewInfo> closedNode)
        {
            if (closedNode != null)
            {
                _workspace.Children.Remove(closedNode.Value.InternalViewInstance);
            }

            if (activatedNode != null)
            {
                PerformActivation(activatedNode, null);
            }

        }
    }
}
