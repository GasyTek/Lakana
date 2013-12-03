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

        protected override void OnBeforePerformTransition(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            if (nodeToDeactivate != null)
            {
                // set the zindex of the node to deactivate at the lowest zindex
                var deactivatedViewGroup = nodeToDeactivate.List;
                var deactivatedViewGroupHost = GroupMappings[deactivatedViewGroup];
                Panel.SetZIndex(deactivatedViewGroupHost, 0);
            }

            if (nodeToActivate != null)
            {
                var activatedViewGroupHost = new ViewGroupHostControl() { Visibility = Visibility.Hidden };
                var activatedViewGroup = nodeToActivate.List;
                var activatedView = nodeToActivate.Value.InternalViewInstance;

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

                // adds the view to the group host
                if (!activatedViewGroupHost.Views.Contains(activatedView))
                    activatedViewGroupHost.Views.Add(activatedView);

                // set the zindex of the node to activate at the hightest zindex
                Panel.SetZIndex(activatedViewGroupHost, 100);
            }
        }

        protected override void OnAfterPerformTransition(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            if (nodeToDeactivate != null)
            {
                // deactivating a view (different of closing a view) means deactivate its view group
                var deactivatedViewGroup = nodeToDeactivate.List;
                var deactivatedViewGroupHost = GroupMappings[deactivatedViewGroup];
                deactivatedViewGroupHost.Visibility = Visibility.Hidden;
            }

            if (nodeToActivate != null)
            {
                var zIndex = 1;
                var activatedViewGroup = nodeToActivate.List;
                var activatedViewGroupHost = GroupMappings[activatedViewGroup];
                var activatedView = nodeToActivate.Value.InternalViewInstance;

                // updates visibility and z-index

                activatedView.Visibility = Visibility.Visible;
                activatedViewGroupHost.Visibility = Visibility.Visible;

                foreach (var view in activatedViewGroup)
                {
                    Panel.SetZIndex(view.InternalViewInstance, zIndex);

                    if (nodeToActivate.Value.IsModal)
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
