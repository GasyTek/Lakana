using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    internal class GridWorkspaceAdapter : WorkspaceAdapterBase<Grid>
    {
        // Maps view groups to their actual framework element equivalent
        private readonly Dictionary<ViewGroup, Grid> _groupMappings;

        public Dictionary<ViewGroup, Grid> GroupMappings
        {
            get { return _groupMappings; }
        }

        public GridWorkspaceAdapter()
        {
            _groupMappings = new Dictionary<ViewGroup, Grid>();
        }

        protected override void OnPerformActivation(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode)
        {
            if (deactivatedNode != null)
            {
                // deactivating a view means deactivate its view group
                var deactivatedViewGroup = deactivatedNode.List;
                var deactivatedContainer = GroupMappings[deactivatedViewGroup];
                deactivatedContainer.Visibility = Visibility.Hidden;
            }

            if (activatedNode != null)
            {
                var zIndex = 1;
                var activatedContainer = new Grid();
                var activatedViewGroup = activatedNode.List;
                var activatedView = activatedNode.Value.InternalViewInstance;

                if(activatedView is Window)
                    throw new ViewTypeNotSupportedByWorkspaceAdapterException(activatedView.GetType());

                if (GroupMappings.ContainsKey(activatedViewGroup))
                {
                    activatedContainer = GroupMappings[activatedViewGroup];
                    activatedContainer.Visibility = Visibility.Visible;
                }
                else
                {
                    Workspace.Children.Add(activatedContainer);
                    GroupMappings.Add(activatedViewGroup, activatedContainer);
                }

                if (!activatedContainer.Children.Contains(activatedView))
                    activatedContainer.Children.Add(activatedView);

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

        protected override void OnPerformClose(ViewGroupNode activatedNode, ViewGroupNode closedNode)
        {
            if (closedNode != null)
            {
                var closedViewGroup = closedNode.List;
                var closedContainer = GroupMappings[closedViewGroup];

                closedContainer.Children.Remove(closedNode.Value.InternalViewInstance);

                if (closedContainer.Children.Count == 0)
                {
                    GroupMappings.Remove(closedViewGroup);
                    Workspace.Children.Remove(closedContainer);
                }
            }

            if (activatedNode != null)
            {
                PerformActivation(activatedNode, null);
            }
        }

        protected override FrameworkElement OnGetViewGroupMapping(ViewGroup viewGroup)
        {
            return GroupMappings.ContainsKey(viewGroup) ? GroupMappings[viewGroup] : null;
        }
    }
}
