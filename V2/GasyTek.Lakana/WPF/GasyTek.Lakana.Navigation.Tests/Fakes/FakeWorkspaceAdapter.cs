using System;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Adapters;
using GasyTek.Lakana.Navigation.Services;
using GasyTek.Lakana.Navigation.Transitions;

namespace GasyTek.Lakana.Navigation.Tests.Fakes
{
    public class FakeWorkspaceAdapter : IWorkspaceAdapter
    {
        public ViewGroupCollectionManager ViewGroupCollectionManager
        {
            get { return null; }
        }

        public void SetMainWorkspace(Panel workspace)
        {
            
        }

        public void SetViewGroupCollection(ViewGroupCollection viewGroupCollection)
        {
        }

        public void SetTransitionAnimationProvider(Func<TransitionAnimation> transitionAnimationProvider)
        {
            
        }

        public void PerformActivation(ViewGroupNode activatedNode, ViewGroupNode deactivatedNode)
        {
        }

        public void PerformClose(ViewGroupNode activatedNode, ViewGroupNode closedNode)
        {
        }
    }
}