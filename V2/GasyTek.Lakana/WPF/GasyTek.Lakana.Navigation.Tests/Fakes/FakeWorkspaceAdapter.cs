using System;
using System.Threading.Tasks;
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

        public Task PerformActivation(ViewGroupNode nodeToDeactivate, ViewGroupNode nodeToActivate)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            return tcs.Task;
        }

        public Task PerformClose(ViewGroupNode nodeToClose, ViewGroupNode nodeToActivate)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            return tcs.Task;
        }
    }
}