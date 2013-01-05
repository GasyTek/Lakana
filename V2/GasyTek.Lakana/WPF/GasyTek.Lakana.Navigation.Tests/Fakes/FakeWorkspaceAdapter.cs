using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Adapters;
using GasyTek.Lakana.Navigation.Services;

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

        public void PerformActivation(LinkedListNode<View> activatedNode, LinkedListNode<View> deactivatedNode)
        {
        }

        public void PerformClose(LinkedListNode<View> activatedNode, LinkedListNode<View> closedNode)
        {
        }
    }
}