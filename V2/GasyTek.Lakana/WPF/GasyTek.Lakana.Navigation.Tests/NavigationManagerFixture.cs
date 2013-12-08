using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Services;
using GasyTek.Lakana.Navigation.Tests.Fakes;
using GasyTek.Lakana.Navigation.Tests.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GasyTek.Lakana.Navigation.Tests
{
    [TestClass]
    public class NavigationManagerFixture
    {
        private NavigationManagerImpl _navigationManagerImpl;
        private UITestHelper _uiTestHelper;

        [TestInitialize]
        public void OnSetup()
        {
            _uiTestHelper = new UITestHelper();

            _uiTestHelper.ExecuteOnUIThread(() =>
            {
                _navigationManagerImpl = new NavigationManagerImpl(new FakeViewLocator());
                _navigationManagerImpl.SetMainWorkspace(new Grid(), new FakeWorkspaceAdapter());
            });
        }

        [TestCleanup]
        public void OnTearDown()
        {
            _uiTestHelper.StopUIThread();
        }

        [TestClass]
        public class NavigateTo : NavigationManagerFixture
        {
            [TestMethod]
            public void CanNavigateToNewView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance,
                        _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.AreEqual(expectedViewInfo.ViewInstanceKey, "view1");
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 1);
                });
            }

            [TestMethod]
            public void CanNavigateToNewParameterizedView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1#1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance,
                        _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.AreEqual(expectedViewInfo.ViewInstanceKey, "view1#1");
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 1);
                });
            }

            [TestMethod]
            public void CanNavigateToExistingView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("view1");

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.AreEqual(expectedViewInfo.ViewInstanceKey, "view1");
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 1);
                });
            }

            [TestMethod]
            public void CanNavigateToExistingParameterizedView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("view1#1");

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1#1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.AreEqual(expectedViewInfo.ViewInstanceKey, "view1#1");
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 1);
                });
            }

            [TestMethod]
            public void CanNavigateToHiddenView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("view1");
                    _navigationManagerImpl.NavigateTo("view2");

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.AreEqual(expectedViewInfo.ViewInstanceKey, "view1");
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 2);
                });
            }

            [TestMethod]
            public void CanStackNewViewOnExistingView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var parentViewInfo = _navigationManagerImpl.NavigateTo("parentView1").View;

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("parentView1/view1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 2);
                    Assert.IsNotNull(_navigationManagerImpl.ActiveNode.Previous);
                    Assert.AreSame(_navigationManagerImpl.ActiveNode.Previous.Value.ViewInstance, parentViewInfo.ViewInstance);
                });
            }

            [TestMethod]
            public void CanNavigateToHiddenStackedView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("parentView1");
                    _navigationManagerImpl.NavigateTo("parentView1/view1");
                    _navigationManagerImpl.NavigateTo("view2");

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("parentView1/view1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.AreEqual(expectedViewInfo.ViewInstanceKey, "view1");
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 3);
                });
            }

            [TestMethod]
            [ExpectedException(typeof(ParentViewInstanceNotFoundException))]
            public void CannotStackNewViewOnNotExistingView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare

                    // Act
                    _navigationManagerImpl.NavigateTo("parentView1/view1");

                    // Verify
                });
            }

            [TestMethod]
            [ExpectedException(typeof(OnlyNewViewInstanceCanBeStackedException))]
            public void CannotStackExistingViewOnDifferentParents()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("parentView1");
                    _navigationManagerImpl.NavigateTo("parentView1/view1");
                    _navigationManagerImpl.NavigateTo("parentView2");

                    // Act
                    _navigationManagerImpl.NavigateTo("parentView2/view1");

                    // Verify
                });
            }

            [TestMethod]
            public void CanStackNewParameterizedViewOnExistingView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var parentViewInfo = _navigationManagerImpl.NavigateTo("parentView1").View;

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("parentView1/view1#1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 2);
                    Assert.IsNotNull(_navigationManagerImpl.ActiveNode.Previous);
                    Assert.AreSame(_navigationManagerImpl.ActiveNode.Previous.Value.ViewInstance, parentViewInfo.ViewInstance);
                });
            }

            [TestMethod]
            public void CanNavigateMoreThanOnceToSameView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("view1");
                    _navigationManagerImpl.NavigateTo("view1");

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 1);
                });
            }

            [TestMethod]
            public void CanNavigateMoreThanOnceToSameStackedView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var parentViewInfo = _navigationManagerImpl.NavigateTo("parentView1").View;
                    _navigationManagerImpl.NavigateTo("parentView1/view1");
                    _navigationManagerImpl.NavigateTo("parentView1/view1");

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("parentView1/view1").View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 2);
                    Assert.IsNotNull(_navigationManagerImpl.ActiveNode.Previous);
                    Assert.AreSame(_navigationManagerImpl.ActiveNode.Previous.Value.ViewInstance, parentViewInfo.ViewInstance);
                });
            }

            [ExpectedException(typeof(ParentViewInstanceNotTopMostException))]
            [TestMethod]
            public void CannotStackNewViewOnNotTopMostView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("parentView1");
                    _navigationManagerImpl.NavigateTo("parentView1/view1");

                    // Act
                    _navigationManagerImpl.NavigateTo("parentView1/view2");

                    // Verify
                });
            }

            [TestMethod]
            public void CanNavigateToNotTopMostView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("parentView1");
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("parentView1/view1").View;
                    _navigationManagerImpl.NavigateTo("view2");

                    // Act
                    _navigationManagerImpl.NavigateTo("parentView1");

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 3);
                });
            }

            [TestMethod]
            public void CanNavigateToNewViewWithViewModel()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var fakeViewModel = new FakeViewModel();

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1", fakeViewModel).View;

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance.DataContext, fakeViewModel);
                });
            }

            #region IViewKeyAware support

            [TestMethod]
            public void CanSupportViewKeyAwareViewModel()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var fakeViewKeyAwareViewModel = new FakeViewModel();

                    // Act
                    _navigationManagerImpl.NavigateTo("view1", fakeViewKeyAwareViewModel);

                    // Verify
                    Assert.AreEqual("view1", fakeViewKeyAwareViewModel.ViewInstanceKey);
                });
            }

            [TestMethod]
            public void CanSupportViewKeyAwareView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1").View;

                    // Verify
                    Assert.IsNotNull(expectedViewInfo.UIMetadata);
                    Assert.AreEqual("ViewLabel", expectedViewInfo.UIMetadata.Label);
                });
            }

            [TestMethod]
            public void CanSupportViewKeyAwareViewModelForStackedView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var fakeViewKeyAwareViewModel = new FakeViewModel();
                    _navigationManagerImpl.NavigateTo("parentView1");

                    // Act
                    _navigationManagerImpl.NavigateTo("parentView1/view1", fakeViewKeyAwareViewModel);

                    // Verify
                    Assert.AreEqual("view1", fakeViewKeyAwareViewModel.ViewInstanceKey);
                });
            }

            [TestMethod]
            public void CanSupportViewKeyAwareViewForStackedView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("parentView1");

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("parentView1/view1").View;

                    // Verify
                    Assert.IsNotNull(expectedViewInfo.UIMetadata);
                    Assert.AreEqual("ViewLabel", expectedViewInfo.UIMetadata.Label);
                });
            }

            #endregion

            #region IPresentable support

            [TestMethod]
            public void CanSupportPresentableViewModel()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var fakePresentableViewModel = new FakeViewModel();

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1", fakePresentableViewModel).View;

                    // Verify
                    Assert.IsNotNull(expectedViewInfo.UIMetadata);
                    Assert.AreEqual("Label", expectedViewInfo.UIMetadata.Label);
                });
            }

            [TestMethod]
            public void CanSupportPresentableView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare

                    // Act
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1").View;

                    // Verify
                    Assert.IsNotNull(expectedViewInfo.UIMetadata);
                    Assert.AreEqual("ViewLabel", expectedViewInfo.UIMetadata.Label);
                });
            }

            #endregion
        }

        [TestClass]
        public class ShowModal : NavigationManagerFixture
        {
            [TestMethod]
            public void CanShowModal()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var parentViewInfo = _navigationManagerImpl.NavigateTo("parentView1").View;

                    // Act
                    var expectedResult = _navigationManagerImpl.ShowModal<object>("parentView1/view1");

                    // Verify
                    Assert.IsTrue(expectedResult.View.IsModal);
                    Assert.AreSame(expectedResult.View.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.IsInstanceOfType(_navigationManagerImpl.ActiveView.ViewInstance, expectedResult.View.ViewInstance.GetType());
                    Assert.IsInstanceOfType(_navigationManagerImpl.ActiveView.ViewHostInstance.View, typeof(ModalHostControl));
                    Assert.IsNotNull(_navigationManagerImpl.ActiveNode.Previous);
                    Assert.AreSame(_navigationManagerImpl.ActiveNode.Previous.Value.ViewInstance, parentViewInfo.ViewInstance);
                });
            }

            [TestMethod]
            [ExpectedException(typeof(NavigationKeyFormatException))]
            public void CannotAcceptNavigationKeyBadFormat()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare

                    // Act
                    _navigationManagerImpl.ShowModal<object>("view1");

                    // Verify
                });
            }

            [TestMethod]
            public void CanRetrieveModalResult()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("parentView1");

                    // Act
                    var modalResult = _navigationManagerImpl.ShowModal<string>("parentView1/view1");
                    _navigationManagerImpl.Close(modalResult.View.ViewInstanceKey, modalResult: "-- Result --");  // Close the modal view and provide the modal result

                    // Verify
                    Assert.AreEqual("-- Result --", modalResult.AsyncResult.Result);
                });
            }
        }

        [TestClass]
        public class ShowMessageBox : NavigationManagerFixture
        {
            [TestMethod]
            public void CanShowMessageBoxDialog()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var parentViewInfo = _navigationManagerImpl.NavigateTo("view1").View;

                    // Act
                    _navigationManagerImpl.ShowMessageBox("view1", "Lorem ipsum", MessageBoxImage.Warning, MessageBoxButton.OKCancel);

                    // Verify
                    Assert.IsTrue(_navigationManagerImpl.ActiveView.IsModal);
                    Assert.IsTrue(_navigationManagerImpl.ActiveView.IsMessageBox);
                    Assert.IsInstanceOfType(_navigationManagerImpl.ActiveView.ViewInstance, typeof(MessageBoxControl));
                    Assert.IsInstanceOfType(_navigationManagerImpl.ActiveView.ViewHostInstance.View, typeof(ModalHostControl));
                    Assert.IsNotNull(_navigationManagerImpl.ActiveNode.Previous);
                    Assert.AreSame(_navigationManagerImpl.ActiveNode.Previous.Value.ViewInstance, parentViewInfo.ViewInstance);
                });
            }
        }

        [TestClass]
        public class Close : NavigationManagerFixture
        {
            [TestMethod]
            public void CanCloseView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var viewInfo = _navigationManagerImpl.NavigateTo("view1").View;

                    // Act
                    var closedViewInfo = _navigationManagerImpl.Close("view1").View;

                    // Verify
                    Assert.IsTrue(_navigationManagerImpl.NbViews == 0);
                    Assert.AreSame(viewInfo.ViewInstance, closedViewInfo.ViewInstance);
                });
            }

            [TestMethod]
            [ExpectedException(typeof(CannotCloseNotTopMostViewException))]
            public void CannotCloseNotTopMostView()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("parentView1");
                    _navigationManagerImpl.NavigateTo("parentView1/view1");

                    // Act
                    _navigationManagerImpl.Close("parentView1");

                    // Verify
                });
            }

            [TestMethod]
            public void CanRestoreLastViewWhenActiveViewIsClosed()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("view2");
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view1").View;
                    _navigationManagerImpl.NavigateTo("view3");

                    // Act
                    _navigationManagerImpl.Close("view3");

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                    Assert.AreEqual(expectedViewInfo.ViewInstanceKey, "view1");
                });
            }

            [TestMethod]
            public void CanKeepActiveViewWhenHiddenViewIsClosed()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    _navigationManagerImpl.NavigateTo("view2");
                    _navigationManagerImpl.NavigateTo("view1");
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("view3").View;

                    // Act
                    _navigationManagerImpl.Close("view2");

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                });
            }

            [TestMethod]
            public void CanRestoreParentViewWhenChildViewIsClosed()
            {
                _uiTestHelper.ExecuteOnUIThread(() =>
                {
                    // Prepare
                    var expectedViewInfo = _navigationManagerImpl.NavigateTo("parentView1").View;
                    _navigationManagerImpl.NavigateTo("parentView1/view1");

                    // Act
                    _navigationManagerImpl.Close("view1");

                    // Verify
                    Assert.AreSame(expectedViewInfo.ViewInstance, _navigationManagerImpl.ActiveView.ViewInstance);
                });
            }
        }

        [TestClass]
        public class CloseApplication : NavigationManagerFixture
        {

        }
    }
}
