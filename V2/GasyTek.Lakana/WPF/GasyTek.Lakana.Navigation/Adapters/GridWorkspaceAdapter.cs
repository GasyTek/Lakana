﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    internal class GridWorkspaceAdapter : WorkspaceAdapterBase<Grid>
    {
        #region Fields

        // Maps view groups to their actual framework element equivalent
        private readonly Dictionary<ViewGroup, ViewGroupHostControl> _groupMappings;

        #endregion
        
        #region Public properties

        public Dictionary<ViewGroup, ViewGroupHostControl> GroupMappings
        {
            get { return _groupMappings; }
        }

        #endregion
        
        #region Constructor

        public GridWorkspaceAdapter()
        {
            _groupMappings = new Dictionary<ViewGroup, ViewGroupHostControl>();
        }

        #endregion

        #region Overriden methods

        protected override void OnBeforeAnimatingActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            AddViewToActivateIfNotExist(nodeToActivate);
            FixZIndex(nodeToDeactivate, nodeToActivate);
        }

        protected override void OnAfterAnimatingActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            // If the node to activate is a modal one 
            // then make its parent visible and disable
            if (nodeToActivate.Value.IsModal)
            {
                var parentView = nodeToActivate.Previous.Value;
                parentView.ViewHostInstance.Visibility = Visibility.Visible;
                parentView.ViewHostInstance.IsEnabled = false;
            }

        }

        protected override void OnBeforeAnimatingClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            // Do nothing
        }

        protected override void OnAfterAnimatingClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            var viewHostToClose = nodeToClose.Value.ViewHostInstance;
            var viewGroupToClose = nodeToClose.List;
            var viewGroupHostToClose = GroupMappings[viewGroupToClose];

            viewGroupHostToClose.Views.Remove(viewHostToClose);

            if (viewGroupHostToClose.Views.Count == 0)
            {
                GroupMappings.Remove(viewGroupToClose);
                Workspace.Children.Remove(viewGroupHostToClose);
            }

            if (nodeToClose.Value.IsModal)
            {
                // Enable the parent of the modal view
                nodeToActivate.Value.ViewHostInstance.IsEnabled = true;
            }
        }

        protected override ViewGroupHostControl OnGetViewGroupHostControl(ViewGroup viewGroup)
        {
            return GroupMappings.ContainsKey(viewGroup) ? GroupMappings[viewGroup] : null;
        }

        #endregion

        #region Private methods

        private void AddViewToActivateIfNotExist(ViewGroupNode nodeToActivate)
        {
            var viewGroupHostToActivate = new ViewGroupHostControl();
            var viewGroupToActivate = nodeToActivate.List;
            var viewHostToActivate = nodeToActivate.Value.ViewHostInstance;

            if (GroupMappings.ContainsKey(viewGroupToActivate))
            {
                viewGroupHostToActivate = GroupMappings[viewGroupToActivate];
            }
            else
            {
                Workspace.Children.Add(viewGroupHostToActivate);
                GroupMappings.Add(viewGroupToActivate, viewGroupHostToActivate);
            }

            // adds the view to the group host
            if (!viewGroupHostToActivate.Views.Contains(viewHostToActivate))
                viewGroupHostToActivate.Views.Add(viewHostToActivate);
        }

        private void FixZIndex(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            if (nodeToDeactivate != null)
            {
                var deactivatedViewGroupHost = GroupMappings[nodeToDeactivate.List];
                Panel.SetZIndex(deactivatedViewGroupHost, 0);
            }

            if (nodeToActivate != null)
            {
                // set the zindex of the node to activate at the hightest zindex
                var activatedViewGroupHost = GroupMappings[nodeToActivate.List];
                Panel.SetZIndex(activatedViewGroupHost, 100);
            }
        }

        #endregion
    }
}
