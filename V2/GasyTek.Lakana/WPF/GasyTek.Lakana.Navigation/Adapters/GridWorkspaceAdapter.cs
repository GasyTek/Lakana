using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    internal class GridWorkspaceAdapter : IWorkspaceAdapter
    {
        private Grid _workspace;
        private ViewStackCollection _viewStackCollection;

        public void SetMainWorkspace(Panel workspace)
        {
            _workspace = (Grid)workspace;
        }

        public void SetViewStackCollection(ViewStackCollection viewStackCollection)
        {
            _viewStackCollection = viewStackCollection;
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
                var activatedView = activatedNode.Value.InternalViewInstance;
                activatedView.Visibility = Visibility.Visible;

                if (!_workspace.Children.Contains(activatedView))
                    _workspace.Children.Add(activatedView);

                if (activatedNode.Value.IsModal)
                {
                    var zIndex = 0;
                    foreach (var viewInfo in activatedNode.List)
                    {
                        viewInfo.InternalViewInstance.Visibility = Visibility.Visible;
                        viewInfo.InternalViewInstance.IsEnabled = viewInfo.InternalViewInstance == activatedView;
                        Panel.SetZIndex(viewInfo.InternalViewInstance, zIndex);
                        zIndex++;
                    }
                }
            }
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
