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
        private GridWorkspaceAdapter _workspaceAdapter;

        [TestInitialize]
        public void OnSetup()
        {
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
                var view = new UserControl();
                var viewGroup = new ViewGroup();
                viewGroup.Push(new View("view1") { InternalViewInstance = view });

                // Act
                _workspaceAdapter.PerformActivation(viewGroup.Peek(), null);

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 1);
                Assert.IsInstanceOfType(_workspace.Children[0], typeof(Grid));
                Assert.IsTrue(((Grid)_workspace.Children[0]).Children.Count == 1);
            }

            [TestMethod]
            public void ActivatedViewIsAddedInExistingGroup()
            {
                // Prepare
                var view1 = new UserControl();
                var view2 = new UserControl();
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                _workspaceAdapter.PerformActivation(view1Node, null);

                // Act
                _workspaceAdapter.PerformActivation(view2Node, null);

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 1);
                Assert.IsInstanceOfType(_workspace.Children[0], typeof(Grid));
                Assert.IsTrue(((Grid)_workspace.Children[0]).Children.Count == 2);
            }

            [TestMethod]
            public void CanActivateViewMoreThanOnce()
            {
                // Prepare
                var view = new UserControl();
                var viewGroup = new ViewGroup();
                viewGroup.Push(new View("view1") { InternalViewInstance = view });
                _workspaceAdapter.PerformActivation(viewGroup.Peek(), null);

                // Act
                _workspaceAdapter.PerformActivation(viewGroup.Peek(), null);

                // Verify
                Assert.IsTrue(((Grid)_workspace.Children[0]).Children.Count == 1);
            }

            [TestMethod]
            public void ActivatedViewHasPositiveZIndexInItsGroup()
            {
                // Prepare
                var view = new UserControl();
                var viewGroup = new ViewGroup();
                viewGroup.Push(new View("view1") { InternalViewInstance = view });

                // Act
                _workspaceAdapter.PerformActivation(viewGroup.Peek(), null);

                // Verify
                Assert.IsTrue(Panel.GetZIndex(view) > 0);
            }

            [TestMethod]
            public void ActivatedViewHasHigherZIndexInItsGroup()
            {
                // Prepare
                var view1 = new UserControl();
                var view2 = new UserControl();
                var view3 = new UserControl();
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                var view3Node = viewGroup.Push(new View("view3") { InternalViewInstance = view3 });
                _workspaceAdapter.PerformActivation(view1Node, null);
                _workspaceAdapter.PerformActivation(view2Node, null);

                // Act
                _workspaceAdapter.PerformActivation(view3Node, null);

                // Verify
                Assert.IsTrue(Panel.GetZIndex(view1) < Panel.GetZIndex(view3));
                Assert.IsTrue(Panel.GetZIndex(view2) < Panel.GetZIndex(view3));

            }

            [TestMethod]
            public void ActivatedModalAncestorsHaveAscendingSortedZIndex()
            {
                // Prepare
                var view1 = new UserControl();
                var view2 = new UserControl();
                var modalView = new UserControl();
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                var modalViewNode = viewGroup.Push(new View("view3") { InternalViewInstance = modalView, IsModal = true });
                _workspaceAdapter.PerformActivation(view1Node, null);
                _workspaceAdapter.PerformActivation(view2Node, null);

                // Act
                _workspaceAdapter.PerformActivation(modalViewNode, null);

                // Verify
                Assert.IsTrue(Panel.GetZIndex(view1) < Panel.GetZIndex(view2));
                Assert.IsTrue(Panel.GetZIndex(view2) < Panel.GetZIndex(modalView));
            }

            [TestMethod]
            public void ActivatedModalAncestorsAreVisibleAndDisabled()
            {
                // Prepare
                var view1 = new UserControl();
                var view2 = new UserControl();
                var modalView = new UserControl();
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                var modalViewNode = viewGroup.Push(new View("view3") { InternalViewInstance = modalView, IsModal = true });
                _workspaceAdapter.PerformActivation(view1Node, null);
                _workspaceAdapter.PerformActivation(view2Node, null);

                // Act
                _workspaceAdapter.PerformActivation(modalViewNode, null);

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
                var view1 = new UserControl();
                var view2 = new UserControl();
                var viewGroup1 = new ViewGroup();
                var viewGroup2 = new ViewGroup();
                viewGroup1.Push(new View("view1") { InternalViewInstance = view1 });
                viewGroup2.Push(new View("view2") { InternalViewInstance = view2 });
                _workspaceAdapter.PerformActivation(viewGroup1.Peek(), null);

                // Act
                _workspaceAdapter.PerformActivation(viewGroup2.Peek(), viewGroup1.Peek());

                // Verify
                Assert.IsTrue(_workspace.Children[0].Visibility == Visibility.Hidden);  // first child corresponds to viewGroup1
            }

            [TestMethod]
            public void ReactivatedViewGroupIsVisible()
            {
                // Prepare
                var view1 = new UserControl();
                var view2 = new UserControl();
                var viewGroup1 = new ViewGroup();
                var viewGroup2 = new ViewGroup();
                viewGroup1.Push(new View("view1") { InternalViewInstance = view1 });
                viewGroup2.Push(new View("view2") { InternalViewInstance = view2 });
                _workspaceAdapter.PerformActivation(viewGroup1.Peek(), null);
                _workspaceAdapter.PerformActivation(viewGroup2.Peek(), viewGroup1.Peek());

                // Act
                _workspaceAdapter.PerformActivation(viewGroup1.Peek(), viewGroup2.Peek());

                // Verify
                Assert.IsTrue(_workspace.Children[0].Visibility == Visibility.Visible);  // first child corresponds to viewGroup1
            }
                        
            [TestMethod]
            [ExpectedException(typeof(ViewTypeNotSupportedByWorkspaceAdapterException))]
            public void ActivatingWindowThrowsException()
            {
                // Prepare
                var view = new Window();
                var viewGroup = new ViewGroup();
                viewGroup.Push(new View("view1") { InternalViewInstance = view });

                // Act
                _workspaceAdapter.PerformActivation(viewGroup.Peek(), null);

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
                var view1 = new UserControl();
                var view2 = new UserControl();
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2 });
                _workspaceAdapter.PerformActivation(view1Node, null);
                _workspaceAdapter.PerformActivation(view2Node, view1Node);

                // Act
                _workspaceAdapter.PerformClose(null, new ViewGroupNode(viewGroup, view2Node.Value));

                // Verify
                Assert.IsTrue(((Grid)_workspace.Children[0]).Children.Count == 1);
            }

            [TestMethod]
            public void ClosedViewGroupIsRemovedIfEmpty()
            {
                // Prepare
                var view1 = new UserControl();
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                _workspaceAdapter.PerformActivation(view1Node, null);

                // Act
                _workspaceAdapter.PerformClose(null, new ViewGroupNode(viewGroup, view1Node.Value));

                // Verify
                Assert.IsTrue(_workspace.Children.Count == 0);
            }

            [TestMethod]
            public void ClosedModalViewParentIsReEnabled()
            {
                // Prepare
                var view1 = new UserControl();
                var view2 = new UserControl();
                var viewGroup = new ViewGroup();
                var view1Node = viewGroup.Push(new View("view1") { InternalViewInstance = view1 });
                var view2Node = viewGroup.Push(new View("view2") { InternalViewInstance = view2, IsModal = true });
                _workspaceAdapter.PerformActivation(view1Node, null);
                _workspaceAdapter.PerformActivation(view2Node, view1Node);

                // Act
                _workspaceAdapter.PerformClose(view1Node, new ViewGroupNode(viewGroup, view2Node.Value));

                // Verify
                Assert.IsTrue(view1.IsEnabled);
            }

        }

    }
}
