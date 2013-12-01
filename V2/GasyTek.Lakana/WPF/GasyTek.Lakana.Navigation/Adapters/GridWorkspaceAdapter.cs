using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    internal class GridWorkspaceAdapter : WorkspaceAdapterBase<Grid>
    {
        // Maps view groups to their actual framework element equivalent
        private readonly Dictionary<ViewGroup, ViewGroupHostControl> _groupMappings;

        public Dictionary<ViewGroup, ViewGroupHostControl> GroupMappings
        {
            get { return _groupMappings; }
        }

        public GridWorkspaceAdapter()
        {
            _groupMappings = new Dictionary<ViewGroup, ViewGroupHostControl>();
        }

        protected override void OnBeforePerformTransition(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode)
        {
            if (activatedNode != null)
            {
                var activatedViewGroupHost = new ViewGroupHostControl();
                var activatedViewGroup = activatedNode.List;
                var activatedView = activatedNode.Value.InternalViewInstance;

                if (GroupMappings.ContainsKey(activatedViewGroup))
                {
                    activatedViewGroupHost = GroupMappings[activatedViewGroup];
                    activatedViewGroupHost.Visibility = Visibility.Visible;
                }
                else
                {
                    Workspace.Children.Add(activatedViewGroupHost);
                    GroupMappings.Add(activatedViewGroup, activatedViewGroupHost);
                }

                if (!activatedViewGroupHost.Views.Contains(activatedView))
                    activatedViewGroupHost.Views.Add(activatedView);
            }
        }

        protected override void OnAfterPerformTransition(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode)
        {
            if (deactivatedNode != null)
            {
                // deactivating a view (different of closing a view) means deactivate its view group
                var deactivatedViewGroup = deactivatedNode.List;
                var deactivatedViewGroupHost = GroupMappings[deactivatedViewGroup];
                deactivatedViewGroupHost.Visibility = Visibility.Hidden;
            }

            if (activatedNode != null)
            {
                var zIndex = 1;
                var activatedViewGroup = activatedNode.List;
                var activatedViewGroupHost = GroupMappings[activatedViewGroup];
                var activatedView = activatedNode.Value.InternalViewInstance;

                // updates visibility and z-index

                activatedView.Visibility = Visibility.Visible;
                activatedViewGroupHost.Visibility = Visibility.Visible;

                foreach (var view in activatedViewGroup)
                {
                    Panel.SetZIndex(view.InternalViewInstance, zIndex);

                    if (activatedNode.Value.IsModal)
                    {
                        view.InternalViewInstance.Visibility = Visibility.Visible;
                    }

                    view.InternalViewInstance.IsEnabled = Equals(activatedView, view.InternalViewInstance);

                    zIndex++;
                }
            }
        }

        protected override Task OnPerformClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (nodeToClose != null)
            {
                var closedViewGroup = nodeToClose.List;
                var closedContainer = GroupMappings[closedViewGroup];

                closedContainer.Views.Remove(nodeToClose.Value.InternalViewInstance);

                if (closedContainer.Views.Count == 0)
                {
                    GroupMappings.Remove(closedViewGroup);
                    Workspace.Children.Remove(closedContainer);
                }
            }

            if (nodeToActivate != null)
            {
                return PerformActivation(null, nodeToActivate);
            }

            tcs.SetResult(true);
            return tcs.Task;
        }

        protected override ViewGroupHostControl OnGetViewGroupMapping(ViewGroup viewGroup)
        {
            return GroupMappings.ContainsKey(viewGroup) ? GroupMappings[viewGroup] : null;
        }
    }
}
