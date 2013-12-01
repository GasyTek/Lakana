using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GasyTek.Lakana.Navigation.Adapters;
using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Navigation.Tests
{
    [TestClass]
    public class GridWorkspaceAdapterFixture
    {
        private Grid _workspace;
        private GridWorkspaceAdapter _workspaceAdapter;

        [TestInitialize]
        public void OnSetup()
        {
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

            _workspace = new Grid();
            _workspaceAdapter = new GridWorkspaceAdapter();
            _workspaceAdapter.SetMainWorkspace(_workspace);
            _workspaceAdapter.SetViewGroupCollection(new ViewGroupCollection());
        }

        [TestClass]
        public class PerformActivation : GridWorkspaceAdapterFixture
        {
            [TestMethod]
            public void ActivatedViewIsAddedInNewGroup()
            {
                // Prepare
                var view = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                viewGroup.Push(new View("view1") { InternalViewInstance = view });

                // Act
                _workspaceAdapter.PerformActivation(null, viewGroup.Peek()).Wait();

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 1);
                Assert.IsInstanceOfType(_workspace.Children[0], typeof(ViewGroupHostControl));
                Assert.IsTrue(((ViewGroupHostControl)_workspace.Children[0]).Views.Count == 1);
            }

            [TestMethod]
            public void ActivatedViewIsAddedInExistingGroup()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var view2 = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                _workspaceAdapter.PerformActivation(null, view1Node).Wait();

                // Act
                _workspaceAdapter.PerformActivation(null, view2Node).Wait();

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 1);
                Assert.IsInstanceOfType(_workspace.Children[0], typeof(ViewGroupHostControl));
                Assert.IsTrue(((ViewGroupHostControl)_workspace.Children[0]).Views.Count == 2);
            }

            [TestMethod]
            public void CanActivateViewMoreThanOnce()
            {
                // Prepare
                var view = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                viewGroup.Push(new View("view1") { InternalViewInstance = view });
                _workspaceAdapter.PerformActivation(null, viewGroup.Peek()).Wait();

                // Act
                _workspaceAdapter.PerformActivation(null, viewGroup.Peek()).Wait();

                // Verify
                Assert.IsTrue(((ViewGroupHostControl)_workspace.Children[0]).Views.Count == 1);
            }

            [TestMethod]
            public void ActivatedViewHasPositiveZIndexInItsGroup()
            {
                // Prepare
                var view = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                viewGroup.Push(new View("view1") { InternalViewInstance = view });

                // Act
                _workspaceAdapter.PerformActivation(null, viewGroup.Peek()).Wait();

                // Verify
                Assert.IsTrue(Panel.GetZIndex(view) > 0);
            }

            [TestMethod]
            public void ActivatedViewHasHigherZIndexInItsGroup()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var view2 = new ViewHostControl { View = new UserControl() };
                var view3 = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                var view3Node = viewGroup.Push(new View("view3") { InternalViewInstance = view3 });
                _workspaceAdapter.PerformActivation(null, view1Node).Wait();
                _workspaceAdapter.PerformActivation(null, view2Node).Wait();

                // Act
                _workspaceAdapter.PerformActivation(null, view3Node).Wait();

                // Verify
                Assert.IsTrue(Panel.GetZIndex(view1) < Panel.GetZIndex(view3));
                Assert.IsTrue(Panel.GetZIndex(view2) < Panel.GetZIndex(view3));

            }

            [TestMethod]
            public void ActivatedModalAncestorsHaveAscendingSortedZIndex()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var view2 = new ViewHostControl { View = new UserControl() };
                var modalView = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                var modalViewNode = viewGroup.Push(new View("view3") { InternalViewInstance = modalView, IsModal = true });
                _workspaceAdapter.PerformActivation(null, view1Node).Wait();
                _workspaceAdapter.PerformActivation(null, view2Node).Wait();

                // Act
                _workspaceAdapter.PerformActivation(null, modalViewNode).Wait();

                // Verify
                Assert.IsTrue(Panel.GetZIndex(view1) < Panel.GetZIndex(view2));
                Assert.IsTrue(Panel.GetZIndex(view2) < Panel.GetZIndex(modalView));
            }

            [TestMethod]
            public void ActivatedModalAncestorsAreVisibleAndDisabled()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var view2 = new ViewHostControl { View = new UserControl() };
                var modalView = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                var modalViewNode = viewGroup.Push(new View("view3") { InternalViewInstance = modalView, IsModal = true });
                _workspaceAdapter.PerformActivation(null, view1Node).Wait();
                _workspaceAdapter.PerformActivation(null, view2Node).Wait();

                // Act
                _workspaceAdapter.PerformActivation(null, modalViewNode).Wait();

                // Verify
                Assert.IsTrue(view1.Visibility == Visibility.Visible);
                Assert.IsFalse(view1.IsEnabled);
                Assert.IsTrue(view2.Visibility == Visibility.Visible);
                Assert.IsFalse(view2.IsEnabled);
                Assert.IsTrue(modalView.Visibility == Visibility.Visible);
                Assert.IsTrue(modalView.IsEnabled);
            }

            [TestMethod]
            public void DeactivatedViewGroupIsNotVisible()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var view2 = new ViewHostControl { View = new UserControl() };
                var viewGroup1 = new ViewGroup();
                var viewGroup2 = new ViewGroup();
                viewGroup1.Push(new View("view1") { InternalViewInstance = view1 });
                viewGroup2.Push(new View("view2") { InternalViewInstance = view2 });
                _workspaceAdapter.PerformActivation(null, viewGroup1.Peek()).Wait();

                // Act
                _workspaceAdapter.PerformActivation(viewGroup1.Peek(), viewGroup2.Peek()).Wait();

                // Verify
                Assert.IsTrue(_workspace.Children[0].Visibility == Visibility.Hidden);  // first child corresponds to viewGroup1
            }

            [TestMethod]
            public void ReactivatedViewGroupIsVisible()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var view2 = new ViewHostControl { View = new UserControl() };
                var viewGroup1 = new ViewGroup();
                var viewGroup2 = new ViewGroup();
                viewGroup1.Push(new View("view1") { InternalViewInstance = view1 });
                viewGroup2.Push(new View("view2") { InternalViewInstance = view2 });
                _workspaceAdapter.PerformActivation(null, viewGroup1.Peek()).Wait();
                _workspaceAdapter.PerformActivation(viewGroup1.Peek(), viewGroup2.Peek()).Wait();

                // Act
                _workspaceAdapter.PerformActivation(viewGroup2.Peek(), viewGroup1.Peek()).Wait();

                // Verify
                Assert.IsTrue(_workspace.Children[0].Visibility == Visibility.Visible);  // first child corresponds to viewGroup1
            }

            [TestMethod]
            [ExpectedException(typeof(ViewTypeNotSupportedByWorkspaceAdapterException))]
            public void ActivatingWindowThrowsException()
            {
                // Prepare
                var view = new ViewHostControl { View = new Window() };
                var viewGroup = new ViewGroup();
                viewGroup.Push(new View("view1") { InternalViewInstance = view });

                // Act
                _workspaceAdapter.PerformActivation(null, viewGroup.Peek()).Wait();

                // Verify
            }
        }

        [TestClass]
        public class PerformClose : GridWorkspaceAdapterFixture
        {
            [TestMethod]
            public void ClosedViewIsRemoved()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var view2 = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                _workspaceAdapter.PerformActivation(null, view1Node).Wait();
                _workspaceAdapter.PerformActivation(view1Node, view2Node).Wait();

                // Act
                _workspaceAdapter.PerformClose(new ViewGroupNode(viewGroup, view2Node.Value), null).Wait();

                // Verify
                Assert.IsTrue(((ViewGroupHostControl)_workspace.Children[0]).Views.Count == 1);
            }

            [TestMethod]
            public void ClosedViewGroupIsRemovedIfEmpty()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                _workspaceAdapter.PerformActivation(null, view1Node).Wait();

                // Act
                _workspaceAdapter.PerformClose(new ViewGroupNode(viewGroup, view1Node.Value), null).Wait();

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 0);
            }

            [TestMethod]
            public void ClosedModalViewParentIsReEnabled()
            {
                // Prepare
                var view1 = new ViewHostControl { View = new UserControl() };
                var view2 = new ViewHostControl { View = new UserControl() };
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2, IsModal = true });
                _workspaceAdapter.PerformActivation(null, view1Node).Wait();
                _workspaceAdapter.PerformActivation(view1Node, view2Node).Wait();
                
                // Act
                _workspaceAdapter.PerformClose(new ViewGroupNode(viewGroup, view2Node.Value), view1Node).Wait();

                // Verify
                Assert.IsTrue(view1.IsEnabled);
            }

        }

    }
}
