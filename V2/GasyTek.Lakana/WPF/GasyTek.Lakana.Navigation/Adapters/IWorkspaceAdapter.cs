using System.Collections.Generic;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Services;

namespace GasyTek.Lakana.Navigation.Adapters
{
    public interface IWorkspaceAdapter
    {
        void SetMainWorkspace(Panel workspace);
        void SetViewStackCollection(ViewStackCollection viewStackCollection);
        void PerformActivation(LinkedListNode<ViewInfo> activatedNode, LinkedListNode<ViewInfo> deactivatedNode);
        void PerformClose(LinkedListNode<ViewInfo> activatedNode, LinkedListNode<ViewInfo> closedNode);
    }
}
