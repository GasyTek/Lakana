using System.Collections.Generic;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Adapters;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Tests.Fakes
{
    public class FakeWorkspaceAdapter : IWorkspaceAdapter
    {
        public ViewStackCollectionManager ViewStackCollectionManager
        {
            get { return null; }
        }

        public void SetMainWorkspace(Panel workspace)
        {
            
        }

        public void SetViewStackCollection(ViewStackCollection viewStackCollection)
        {
        }

        public void PerformActivation(LinkedListNode<ViewInfo> activatedNode, LinkedListNode<ViewInfo> deactivatedNode)
        {
        }

        public void PerformClose(LinkedListNode<ViewInfo> activatedNode, LinkedListNode<ViewInfo> closedNode)
        {
        }
    }
}