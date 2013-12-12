using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Adapters;
using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Services;
using GasyTek.Lakana.Navigation.Tests.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Navigation.Tests
{
    [TestClass]
    public class GridWorkspaceAdapterFixture
    {
        private Grid _workspace;
        private GridWorkspaceAdapter _workspaceAdapter;
        private UITestHelper _uiTestHelper;

        [TestInitialize]
        public void OnSetup()
        {
            _uiTestHelper = new UITestHelper();

            _uiTestHelper.ExecuteOnUIThread(() =>
                    {
                        _workspace = new Grid();
                        _workspaceAdapter = new GridWorkspaceAdapter();
                        //_workspaceAdapter.SetTransitionAnimationProvider(TransitionAnimation.Create);
                        _workspaceAdapter.SetMainWorkspace(_workspace);
                        _workspaceAdapter.SetViewGroupCollection(new ViewGroupCollection());
                    });
        }

        [TestCleanup]
        public void OnTearDown()
        {
            _uiTestHelper.StopUIThread();
        }

        [TestClass]
        public class PerformActivation : GridWorkspaceAdapterFixture
        {
            [TestMethod]
            public void ActivatedViewIsAddedInNewGroup()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view = new ViewHostControl { View = new UserControl() };
                    var viewGroup = new ViewGroup();
                    viewGroup.Push(new View("view1") { ViewHostInstance = view });

                    // Act
                    _workspaceAdapter.PerformUIActivation(null, viewGroup.Peek()).Wait();

                    // Verify
                    Assert.IsTrue(_workspace.Children.Count == 1);
                    Assert.IsInstanceOfType(_workspace.Children[0], typeof(ViewGroupHostControl));
                    Assert.IsTrue(((ViewGroupHostControl)_workspace.Children[0]).Views.Count == 1);
                });
            }

            [TestMethod]
            public void ActivatedViewIsAddedInExistingGroup()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view1 = new ViewHostControl { View = new UserControl() };
                    var view2 = new ViewHostControl { View = new UserControl() };
                    var viewGroup = new ViewGroup();
                    var view1Node = viewGroup.Push(new View("view1") { ViewHostInstance = view1 });
                    var view2Node = viewGroup.Push(new View("view2") { ViewHostInstance = view2 });
                    _workspaceAdapter.PerformUIActivation(null, view1Node).Wait();

                    // Act
                    _workspaceAdapter.PerformUIActivation(null, view2Node).Wait();

                    // Verify
                    Assert.IsTrue(_workspace.Children.Count == 1);
                    Assert.IsInstanceOfType(_workspace.Children[0], typeof(ViewGroupHostControl));
                    Assert.IsTrue(((ViewGroupHostControl)_workspace.Children[0]).Views.Count == 2);
                });
            }

            [TestMethod]
            public void CanActivateViewMoreThanOnce()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view = new ViewHostControl { View = new UserControl() };
                    var viewGroup = new ViewGroup();
                    viewGroup.Push(new View("view1") { ViewHostInstance = view });
                    _workspaceAdapter.PerformUIActivation(null, viewGroup.Peek()).Wait();

                    // Act
                    _workspaceAdapter.PerformUIActivation(null, viewGroup.Peek()).Wait();

                    // Verify
                    Assert.IsTrue(((ViewGroupHostControl)_workspace.Children[0]).Views.Count == 1);
                });
            }

            [TestMethod]
            public void ActivatedModalAncestorsAreVisibleAndDisabled()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view1 = new ViewHostControl { View = new UserControl() };
                    var view2 = new ViewHostControl { View = new UserControl() };
                    var modalView = new ViewHostControl { View = new UserControl() };
                    var viewGroup = new ViewGroup();
                    var view1Node = viewGroup.Push(new View("view1") { ViewHostInstance = view1 });
                    var view2Node = viewGroup.Push(new View("view2") { ViewHostInstance = view2, IsModal = true });
                    var modalViewNode =
                        viewGroup.Push(new View("view3") { ViewHostInstance = modalView, IsModal = true });
                    _workspaceAdapter.PerformUIActivation(null, view1Node).Wait();
                    _workspaceAdapter.PerformUIActivation(null, view2Node).Wait();

                    // Act
                    _workspaceAdapter.PerformUIActivation(null, modalViewNode).Wait();

                    // Verify
                    Assert.IsTrue(view1.Visibility == Visibility.Visible);
                    Assert.IsFalse(view1.IsEnabled);
                    Assert.IsTrue(view2.Visibility == Visibility.Visible);
                    Assert.IsFalse(view2.IsEnabled);
                    Assert.IsTrue(modalView.Visibility == Visibility.Visible);
                    Assert.IsTrue(modalView.IsEnabled);
                });
            }

            [TestMethod]
            public void DeactivatedViewGroupIsNotVisible()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view1 = new ViewHostControl { View = new UserControl() };
                    var view2 = new ViewHostControl { View = new UserControl() };
                    var viewGroup1 = new ViewGroup();
                    var viewGroup2 = new ViewGroup();
                    viewGroup1.Push(new View("view1") { ViewHostInstance = view1 });
                    viewGroup2.Push(new View("view2") { ViewHostInstance = view2 });
                    _workspaceAdapter.PerformUIActivation(null, viewGroup1.Peek()).Wait();

                    // Act
                    _workspaceAdapter.PerformUIActivation(viewGroup1.Peek(), viewGroup2.Peek()).Wait();

                    // Verify
                    Assert.IsTrue(_workspace.Children[0].Visibility == Visibility.Hidden);
                    // first child corresponds to viewGroup1
                });
            }

            [TestMethod]
            public void ReactivatedViewGroupIsVisible()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view1 = new ViewHostControl { View = new UserControl() };
                    var view2 = new ViewHostControl { View = new UserControl() };
                    var viewGroup1 = new ViewGroup();
                    var viewGroup2 = new ViewGroup();
                    viewGroup1.Push(new View("view1") { ViewHostInstance = view1 });
                    viewGroup2.Push(new View("view2") { ViewHostInstance = view2 });
                    _workspaceAdapter.PerformUIActivation(null, viewGroup1.Peek()).Wait();
                    _workspaceAdapter.PerformUIActivation(viewGroup1.Peek(), viewGroup2.Peek()).Wait();

                    // Act
                    _workspaceAdapter.PerformUIActivation(viewGroup2.Peek(), viewGroup1.Peek()).Wait();

                    // Verify
                    Assert.IsTrue(_workspace.Children[0].Visibility == Visibility.Visible);
                    // first child corresponds to viewGroup1
                });
            }

            [TestMethod]
            [ExpectedException(typeof(ViewTypeNotSupportedByWorkspaceAdapterException))]
            public void ActivatingWindowThrowsException()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view = new ViewHostControl { View = new Window() };
                    var viewGroup = new ViewGroup();
                    viewGroup.Push(new View("view1") { ViewHostInstance = view });

                    // Act
                    _workspaceAdapter.PerformUIActivation(null, viewGroup.Peek()).Wait();

                    // Verify
                });
            }
        }

        [TestClass]
        public class PerformClose : GridWorkspaceAdapterFixture
        {
            [TestMethod]
            public void ClosedViewIsRemoved()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {

                    // Prepare
                    var view1 = new ViewHostControl { View = new UserControl() };
                    var view2 = new ViewHostControl { View = new UserControl() };
                    var viewGroup = new ViewGroup();
                    var view1Node = viewGroup.Push(new View("view1") { ViewHostInstance = view1 });
                    var view2Node = viewGroup.Push(new View("view2") { ViewHostInstance = view2 });
                    _workspaceAdapter.PerformUIActivation(null, view1Node).Wait();
                    _workspaceAdapter.PerformUIActivation(view1Node, view2Node).Wait();

                    // Act
                    _workspaceAdapter.PerformUIClose(new ViewGroupNode(viewGroup, view2Node.Value), null)
                        .Wait();

                    // Verify
                    Assert.IsTrue(((ViewGroupHostControl)_workspace.Children[0]).Views.Count == 1);
                });
            }

            [TestMethod]
            public void ClosedViewGroupIsRemovedIfEmpty()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view1 = new ViewHostControl { View = new UserControl() };
                    var viewGroup = new ViewGroup();
                    var view1Node = viewGroup.Push(new View("view1") { ViewHostInstance = view1 });
                    _workspaceAdapter.PerformUIActivation(null, view1Node).Wait();

                    // Act
                    _workspaceAdapter.PerformUIClose(new ViewGroupNode(viewGroup, view1Node.Value), null)
                        .Wait();

                    // Verify
                    Assert.IsTrue(_workspace.Children.Count == 0);
                });
            }

            [TestMethod]
            public void ClosedModalViewParentIsReEnabled()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var view1 = new ViewHostControl { View = new UserControl() };
                    var view2 = new ViewHostControl { View = new UserControl() };
                    var viewGroup = new ViewGroup();
                    var view1Node = viewGroup.Push(new View("view1") { ViewHostInstance = view1 });
                    var view2Node = viewGroup.Push(new View("view2") { ViewHostInstance = view2, IsModal = true });
                    _workspaceAdapter.PerformUIActivation(null, view1Node).Wait();
                    _workspaceAdapter.PerformUIActivation(view1Node, view2Node).Wait();

                    // Act
                    _workspaceAdapter.PerformUIClose(new ViewGroupNode(viewGroup, view2Node.Value), view1Node).Wait();

                    // Verify
                    Assert.IsTrue(view1.IsEnabled);
                });
            }
        }
    }
}
