using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Adapters;
using GasyTek.Lakana.Navigation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Navigation.Tests
{
    [TestClass]
    public class GridWorkspaceAdapterFixture
    {
        private Grid _workspace;
        private ViewStackCollection _viewStackCollection;
        private GridWorkspaceAdapter _workspaceAdapter;

        [TestInitialize]
        public void OnSetup()
        {
            _workspace = new Grid();
            _viewStackCollection = new ViewStackCollection();
            _workspaceAdapter = new GridWorkspaceAdapter();
            _workspaceAdapter.SetMainWorkspace(_workspace);
            _workspaceAdapter.SetViewStackCollection(_viewStackCollection);
        }

        [TestClass]
        public class PerformActivation : GridWorkspaceAdapterFixture
        {
            [TestMethod]
            public void ActivatedViewIsAddedIfNew()
            {
                // Prepare
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view });

                // Act
                _workspaceAdapter.PerformActivation(viewStack.Last, null);

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 1);
            }

            [TestMethod]
            public void ActivatedViewHasHigherZIndex()
            {
                // Prepare
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view });

                // Act
                _workspaceAdapter.PerformActivation(viewStack.Last, null);

                // Verify
                Assert.IsTrue(Panel.GetZIndex(_workspace.Children[0]) > 0);
            }

            [TestMethod]
            public void ActivatedModalAncestorsHaveAscendingSortedZIndex()
            {
                // Prepare
                var parentView = new UserControl();
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("parentView1") { InternalViewInstance = parentView });
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view, IsModal = true });

                // Act
                _workspaceAdapter.PerformActivation(viewStack.Last, null);

                // Verify
                Assert.IsTrue(Panel.GetZIndex(parentView) < Panel.GetZIndex(view));

            }
            
            [TestMethod]
            public void ActivatedModalAncestorsVisibleAndDisabled()
            {
                // Prepare
                var parentView = new UserControl();
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("parentView1") { InternalViewInstance = parentView });
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view, IsModal = true });

                // Act
                _workspaceAdapter.PerformActivation(viewStack.Last, null);

                // Verify
                Assert.IsTrue(parentView.Visibility == Visibility.Visible);
                Assert.IsFalse(parentView.IsEnabled);
                Assert.IsTrue(view.Visibility == Visibility.Visible);
                Assert.IsTrue(view.IsEnabled);
            }

            [TestMethod]
            public void DeactivatedViewIsNotVisible()
            {
                // Prepare
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view });
                _workspaceAdapter.PerformActivation(viewStack.Last, null);

                // Act
                _workspaceAdapter.PerformActivation(null, viewStack.Last);

                // Verify
                Assert.IsTrue(view.Visibility == Visibility.Hidden);
            }

            [TestMethod]
            public void DeactivateModalViewShouldHideItsStack()
            {
                // Prepare
                var parentView = new UserControl();
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("parentView1") { InternalViewInstance = parentView });
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view, IsModal = true });
                _workspaceAdapter.PerformActivation(viewStack.Last, null);

                // Act
                _workspaceAdapter.PerformActivation(null, viewStack.Last);

                // Verify
                Assert.IsTrue(parentView.Visibility == Visibility.Hidden);
                Assert.IsTrue(view.Visibility == Visibility.Hidden);
            }

            [TestMethod]
            public void PreviouslyDeactivatedViewIsMadeVisibleIfReactivated()
            {
                // Prepare
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view });
                _workspaceAdapter.PerformActivation(viewStack.Last, null);
                _workspaceAdapter.PerformActivation(null, viewStack.Last);

                // Act
                _workspaceAdapter.PerformActivation(viewStack.Last, null);

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 1);
                Assert.IsTrue(view.Visibility == Visibility.Visible);
            }
        }

        [TestClass]
        public class PerformClose : GridWorkspaceAdapterFixture
        {
            [TestMethod]
            public void ClosedViewIsRemoved()
            {
                // Prepare
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view });
                _workspaceAdapter.PerformActivation(viewStack.Last, null);

                // Act
                _workspaceAdapter.PerformClose(null, viewStack.Last);

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 0);
            }

            [TestMethod]
            public void ClosedModalViewParentIsReEnabled()
            {
                // Prepare
                var parentView = new UserControl();
                var view = new UserControl();
                var viewStack = new ViewStack();
                viewStack.AddLast(new ViewInfo("parentView1") { InternalViewInstance = parentView });
                _workspaceAdapter.PerformActivation(viewStack.Last, null);
                viewStack.AddLast(new ViewInfo("view1") { InternalViewInstance = view, IsModal = true });
                _workspaceAdapter.PerformActivation(viewStack.Last, viewStack.First);

                // Act
                _workspaceAdapter.PerformClose(viewStack.First, viewStack.Last);

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 1);
                Assert.IsTrue(parentView.IsEnabled);
            }

        }

    }
}
