using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.WPF.Controls;
using GasyTek.Lakana.WPF.Services;
using GasyTek.Lakana.WPF.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GasyTek.Lakana.WPF.Tests
{
    [TestClass]
    public class NavigationServiceFixture
    {
        private INavigationService _navigationService;

        [TestInitialize]
        public void OnSetup()
        {
            _navigationService = new NavigationService();
            _navigationService.Initialize(new Grid());
        }

        [TestClass]
        public class Initialize : NavigationServiceFixture
        {
            [TestMethod]
            public void CanInitializeNavigationService()
            {
                // Prepare
                var panel = new Grid();

                // Act
                _navigationService.Initialize(panel);

                // Verify
                Assert.IsNotNull(_navigationService.RootPanel);
            }
        }

        [TestClass]
        public class NavigateTo : NavigationServiceFixture
        {
            [TestMethod]
            public void CanNavigateToNewView()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");

                // Act
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                Assert.AreSame(expectedViewInfo.View, _navigationService.CurrentView.View);
                Assert.IsTrue(_navigationService.RootPanel.Children.Contains(expectedViewInfo.View));
            }

            [TestMethod]
            public void CanNavigateManySuccessiveTimesToTheSameView()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                _navigationService.NavigateTo<UserControl>(navigationInfo);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                Assert.AreSame(expectedViewInfo.View, _navigationService.CurrentView.View);
                Assert.IsTrue(_navigationService.RootPanel.Children.Contains(expectedViewInfo.View));
                Assert.IsTrue(_navigationService.NbOpenedViews == 1);
                Assert.IsTrue(expectedViewInfo.View.Visibility == Visibility.Visible);
            }

            [TestMethod]
            public void CanNavigateToExistingView()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateSimple("viewKey2");
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo1);
                var oldViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo1);

                // Verify
                Assert.AreSame(expectedViewInfo.View, _navigationService.CurrentView.View);
                Assert.IsTrue(_navigationService.NbOpenedViews == 2);
                Assert.IsTrue(expectedViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsTrue(oldViewInfo.View.Visibility == Visibility.Collapsed);
            }

            [TestMethod]
            public void CanNavigateToExistingViewUsingViewKey()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateSimple("viewKey2");
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                var oldViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo("viewKey1");

                // Verify
                Assert.AreSame(expectedViewInfo.View, _navigationService.CurrentView.View);
                Assert.IsTrue(_navigationService.RootPanel.Children.Contains(expectedViewInfo.View));
                Assert.IsTrue(expectedViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsTrue(oldViewInfo.View.Visibility == Visibility.Collapsed);
            }

            [ExpectedException(typeof(ViewNotFoundException))]
            [TestMethod]
            public void CannotNavigateToNonExistantViewUsingViewKeyOnly()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateSimple("viewKey2");
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Act
                _navigationService.NavigateTo("nonExistantViewKey");

                // Verify
            }

            [TestMethod]
            public void CanNavigateAndAttachNewViewWithParentView()
            {
                // Prepare
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var childNavigationInfo = NavigationInfo.CreateComplex("childViewKey", parentNavigationInfo.ViewKey);
                var oldViewInfo = _navigationService.NavigateTo<UserControl>(parentNavigationInfo);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(childNavigationInfo);

                // Verify
                Assert.AreSame(expectedViewInfo.View, _navigationService.CurrentView.View);
                Assert.IsTrue(_navigationService.RootPanel.Children.Contains(expectedViewInfo.View));
                Assert.IsTrue(expectedViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsTrue(oldViewInfo.View.Visibility == Visibility.Collapsed);
            }

            [TestMethod]
            public void CanNavigateAndAttachSameViewToSameParentManySuccessiveTimes()
            {
                // Prepare
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var childNavigationInfo = NavigationInfo.CreateComplex("childViewKey", parentNavigationInfo.ViewKey);
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);
                var childViewInfo = _navigationService.NavigateTo<UserControl>(childNavigationInfo);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(childNavigationInfo);

                // Verify
                Assert.AreEqual(expectedViewInfo, _navigationService.CurrentView);
                Assert.AreSame(expectedViewInfo.View, childViewInfo.View);
            }

            [ExpectedException(typeof(ParentViewNotTopMostException))]
            [TestMethod]
            public void CannotNavigateAndAttachNewViewWithNotTopMostParentView()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateComplex("viewKey2", navigationInfo1.ViewKey);
                var navigationInfo3 = NavigationInfo.CreateComplex("viewKey3", navigationInfo1.ViewKey);
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo3);

                // Verify
            }

            [ExpectedException(typeof(ChildViewAlreadyExistsException))]
            [TestMethod]
            public void CannotNavigateAndAttachExistingViewWithParentView()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                _navigationService.NavigateTo<UserControl>(navigationInfo);
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);

                // Act
                var notAllowedNavigationInfo = NavigationInfo.CreateComplex(navigationInfo.ViewKey, parentNavigationInfo.ViewKey);
                _navigationService.NavigateTo<UserControl>(notAllowedNavigationInfo);

                // Verify
            }

            [TestMethod]
            public void NavigateToNotTopMostViewMakesTheTopMostViewOfTheStackTheCurrentView()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateComplex("viewKey2", navigationInfo1.ViewKey);
                var navigationInfo3 = NavigationInfo.CreateComplex("viewKey3", navigationInfo2.ViewKey);
                var oldViewInfo1 = _navigationService.NavigateTo<UserControl>(navigationInfo1);
                var oldViewInfo2 = _navigationService.NavigateTo<UserControl>(navigationInfo2);
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo3);

                // Act
                _navigationService.NavigateTo("viewKey1");

                // Verify
                Assert.AreSame(expectedViewInfo.View, _navigationService.CurrentView.View);
                Assert.IsTrue(_navigationService.RootPanel.Children.Contains(expectedViewInfo.View));
                Assert.IsTrue(expectedViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsTrue(oldViewInfo1.View.Visibility == Visibility.Collapsed);
                Assert.IsTrue(oldViewInfo2.View.Visibility == Visibility.Collapsed);
            }

            [TestMethod]
            public void CanNavigateToNewViewWithViewModel()
            {
                // Prepare
                var fakeViewModel = new FakeViewModel();
                var navigationInfo = NavigationInfo.CreateSimple("viewKey", fakeViewModel);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                Assert.IsNotNull(expectedViewInfo.View.DataContext);
            }

            [TestMethod]
            public void CanNavigateToNewViewWithViewKeyAwareViewModel()
            {
                // Prepare
                var fakeViewModel = new FakeViewKeyAwareViewModel();
                var navigationInfo = NavigationInfo.CreateSimple("viewKey", fakeViewModel);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                Assert.IsNotNull(expectedViewInfo.View.DataContext);
                Assert.AreEqual(navigationInfo.ViewKey, ((IViewKeyAware)expectedViewInfo.View.DataContext).ViewKey);
            }

            [TestMethod]
            public void CanNavigateAndAttachNewViewWithViewKeyAwareViewModelToParentView()
            {
                // Prepare
                var fakeViewModel = new FakeViewKeyAwareViewModel();
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var childNavigationInfo = NavigationInfo.CreateComplex("childViewKey", parentNavigationInfo.ViewKey, fakeViewModel);
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(childNavigationInfo);

                // Verify
                Assert.IsNotNull(expectedViewInfo.View.DataContext);
                Assert.AreEqual(childNavigationInfo.ViewKey, ((IViewKeyAware)expectedViewInfo.View.DataContext).ViewKey);
            }

            [TestMethod]
            public void CanNavigateToModalView()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var modalNavigationInfo = NavigationInfo.CreateComplex("modalViewKey", parentNavigationInfo.ViewKey);
                var parentViewInfo = _navigationService.NavigateTo<UserControl>(parentNavigationInfo);
                _navigationService.ShowModal<UserControl, string>(modalNavigationInfo);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(modalNavigationInfo);

                // Verify
                Assert.AreEqual(expectedViewInfo, _navigationService.CurrentView);
                Assert.IsTrue(expectedViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsTrue(parentViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsFalse(parentViewInfo.View.IsEnabled);
                Assert.IsTrue(_navigationService.NbOpenedViews == 3);
            }

            [TestMethod]
            public void CanNavigateToModalViewUsingViewKey()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var modalNavigationInfo = NavigationInfo.CreateComplex("modalViewKey", parentNavigationInfo.ViewKey);
                _navigationService.NavigateTo<UserControl>(navigationInfo);
                var parentViewInfo = _navigationService.NavigateTo<UserControl>(parentNavigationInfo);
                _navigationService.ShowModal<UserControl, bool>(modalNavigationInfo);

                // Act
                var expectedViewInfo = _navigationService.NavigateTo("modalViewKey");

                // Verify
                Assert.AreEqual(expectedViewInfo, _navigationService.CurrentView);
                Assert.IsTrue(expectedViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsTrue(parentViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsFalse(parentViewInfo.View.IsEnabled);
            }

            [TestMethod]
            public void CanRestoreModalViewWhenNavigatingToNotTopMostView()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var modalNavigationInfo = NavigationInfo.CreateComplex("modalViewKey", parentNavigationInfo.ViewKey);
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);
                var modalResult = _navigationService.ShowModal<UserControl, bool>(modalNavigationInfo);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                var parentViewInfo = _navigationService.NavigateTo<UserControl>(parentNavigationInfo);

                // Verify
                Assert.AreEqual(modalResult.ViewInfo, _navigationService.CurrentView);
                Assert.IsTrue(modalResult.ViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsTrue(parentViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsFalse(parentViewInfo.View.IsEnabled);
            }

            [TestMethod]
            public void NavigateToActiveAwareViewSupported()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var navigationInfo = NavigationInfo.CreateSimple("viewKey", activeAwareViewModelMock.Object);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(1));
            }

            [TestMethod]
            public void NavigateToExistingActiveAwareViewSupported()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var navigationInfo = NavigationInfo.CreateSimple("viewKey", activeAwareViewModelMock.Object);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                _navigationService.NavigateTo("viewKey");

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(2));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(2));
            }

            [TestMethod]
            public void NavigateToActiveAwareViewOnTopOfExistingOneSupported()
            {
                // Prepare
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var navigationInfo = NavigationInfo.CreateComplex("viewKey", "parentViewKey", activeAwareViewModelMock.Object);
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(1));
            }

            [TestMethod]
            public void NavigateMultipleTimeToActiveAwareViewSupported()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var navigationInfo = NavigationInfo.CreateSimple("viewKey", activeAwareViewModelMock.Object);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(2));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(2));
            }

            [TestMethod]
            public void NavigateMultipleTimeToActiveAwareViewOnTopOfExistingOneSupported()
            {
                // Prepare
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var navigationInfo = NavigationInfo.CreateComplex("viewKey", "parentViewKey", activeAwareViewModelMock.Object);
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(2));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(2));
            }

            [TestMethod]
            public void NavigateToExistingNotTopMostViewDoNotFireActivation()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey", activeAwareViewModelMock.Object);
                var navigationInfo = NavigationInfo.CreateComplex("viewKey", "parentViewKey");
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                _navigationService.NavigateTo("parentViewKey");

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(1));
            }

            [TestMethod]
            public void NavigateMultipleTimeToNotTopMostViewDoNotFireActivation()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey", activeAwareViewModelMock.Object);
                var navigationInfo = NavigationInfo.CreateComplex("viewKey", "parentViewKey");
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(1));
            }

            [TestMethod]
            public void NavigateMultipleTimeToNotTopMostParentViewDoNotFireActivation()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var childNavigationInfo = NavigationInfo.CreateComplex("childViewKey", "parentViewKey", activeAwareViewModelMock.Object);
                var grandChildNavigationInfo = NavigationInfo.CreateComplex("grandChildViewKey", "childViewKey");
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);
                _navigationService.NavigateTo<UserControl>(childNavigationInfo);
                _navigationService.NavigateTo<UserControl>(grandChildNavigationInfo);

                // Act
                _navigationService.NavigateTo<UserControl>(childNavigationInfo);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(1));    
            }

            [TestMethod]
            public void LeavingActiveAwareViewSupported()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1", activeAwareViewModelMock.Object);
                var navigationInfo2 = NavigationInfo.CreateSimple("viewKey2");
                _navigationService.NavigateTo<UserControl>(navigationInfo1);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnDeactivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnDeactivated(), Times.Exactly(1));
            }

            [TestMethod]
            public void LeavingExistingActiveAwareViewSupported()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var navigationInfo2 = NavigationInfo.CreateSimple("viewKey2", activeAwareViewModelMock.Object);
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo1);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnDeactivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnDeactivated(), Times.Exactly(1));
            }

            [TestMethod]
            public void LeavingParentActiveAwareViewSupported()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey", activeAwareViewModelMock.Object);
                var navigationInfo = NavigationInfo.CreateComplex("viewKey", "parentViewKey");
                _navigationService.NavigateTo<UserControl>(parentNavigationInfo);

                // Act
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnDeactivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnDeactivated(), Times.Exactly(1));
            }
        }

        [TestClass]
        public class ShowMessageBox : NavigationServiceFixture
        {
            [TestMethod]
            public void CanShowMessageBoxDialog()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var oldViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                _navigationService.ShowMessageBox("viewKey", "Lorem ipsum", MessageBoxImage.Warning, MessageBoxButton.OKCancel);

                // Verify
                Assert.IsInstanceOfType(_navigationService.CurrentView.View, typeof(ModalHostControl));
                Assert.IsTrue(oldViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsFalse(string.IsNullOrEmpty(((MessageBoxControl)((ModalHostControl)_navigationService.CurrentView.View).ModalContent).Message));
            }
        }

        [TestClass]
        public class ShowModal : NavigationServiceFixture
        {
            [TestMethod]
            public void CanShowModal()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var modalNavigationInfo = NavigationInfo.CreateComplex("modalViewKey", navigationInfo.ViewKey);
                var oldViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                var modalResult = _navigationService.ShowModal<UserControl, bool>(modalNavigationInfo);

                // Verify
                Assert.AreEqual(modalResult.ViewInfo, _navigationService.CurrentView);
                Assert.IsInstanceOfType(_navigationService.CurrentView.View, typeof(ModalHostControl));
                Assert.IsTrue(oldViewInfo.View.Visibility == Visibility.Visible);
            }

            [TestMethod]
            public void WhenModalViewIsDisplayedItsParentIsVisibleAndDisabled()
            {
                // Prepare
                var parentNavigationInfo = NavigationInfo.CreateSimple("parentViewKey");
                var modalNavigationInfo = NavigationInfo.CreateComplex("modalViewKey", parentNavigationInfo.ViewKey);
                var parentViewInfo = _navigationService.NavigateTo<UserControl>(parentNavigationInfo);

                // Act
                var modalResult = _navigationService.ShowModal<UserControl, bool>(modalNavigationInfo);

                // Verify
                Assert.IsTrue(modalResult.ViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsTrue(parentViewInfo.View.Visibility == Visibility.Visible);
                Assert.IsFalse(parentViewInfo.View.IsEnabled);
            }

            [TestMethod]
            public void CanRetrieveModalResult()
            {
                // Prepare
                var testedModalResult = "";
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var modalNavigationInfo = NavigationInfo.CreateComplex("modalViewKey", navigationInfo.ViewKey);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                var modalResult = _navigationService.ShowModal<UserControl, string>(modalNavigationInfo);
                modalResult.Result.ContinueWith(r => testedModalResult = r.Result);
                _navigationService.Close(modalResult.ViewInfo.ViewKey, "ModalResult");  // Close the modal view and provide the modal result

                // Verify
                Assert.AreEqual("ModalResult", testedModalResult);
            }

            [TestMethod]
            public void ShowModalActiveAwareViewSupported()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var modalNavigationInfo = NavigationInfo.CreateComplex("modalViewKey", navigationInfo.ViewKey, activeAwareViewModelMock.Object);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                _navigationService.ShowModal<UserControl, string>(modalNavigationInfo);
                
                // Verify
                activeAwareViewModelMock.Verify(m => m.OnActivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnActivated(), Times.Exactly(1));

            }
        }

        [TestClass]
        public class Close : NavigationServiceFixture
        {
            [TestMethod]
            public void CanCloseView()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey");
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                var closedViewInfo = _navigationService.Close("viewKey");

                // Verify
                Assert.AreSame(expectedViewInfo.View, closedViewInfo.View);
                Assert.IsTrue(_navigationService.NbOpenedViews == 0);
            }

            [ExpectedException(typeof(ParentViewNotTopMostException))]
            [TestMethod]
            public void CannotCloseNotTopMostView()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateComplex("viewKey2", navigationInfo1.ViewKey);
                var navigationInfo3 = NavigationInfo.CreateComplex("viewKey3", navigationInfo2.ViewKey);
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                _navigationService.NavigateTo<UserControl>(navigationInfo2);
                _navigationService.NavigateTo<UserControl>(navigationInfo3);

                // Act
                _navigationService.Close("viewKey2");

                // Verify
            }

            [TestMethod]
            public void ClosingCurrentViewRestoresOldCurrentView()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateSimple("viewKey2");
                var navigationInfo3 = NavigationInfo.CreateSimple("viewKey3");
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo2);
                _navigationService.NavigateTo<UserControl>(navigationInfo3);

                // Act
                var closedViewInfo = _navigationService.Close("viewKey3");

                // Verify
                Assert.AreSame(expectedViewInfo.View, _navigationService.CurrentView.View);
                Assert.IsFalse(_navigationService.RootPanel.Children.Contains(closedViewInfo.View));
                Assert.IsTrue(_navigationService.NbOpenedViews == 2);
            }

            [TestMethod]
            public void ClosingCurrentViewRestoresParentView()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateSimple("viewKey2", navigationInfo1.ViewKey);
                var navigationInfo3 = NavigationInfo.CreateSimple("viewKey3", navigationInfo2.ViewKey);
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                var expectedViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo2);
                _navigationService.NavigateTo<UserControl>(navigationInfo3);

                // Act
                var closedViewInfo = _navigationService.Close("viewKey3");

                // Verify
                Assert.AreSame(expectedViewInfo.View, _navigationService.CurrentView.View);
                Assert.IsFalse(_navigationService.RootPanel.Children.Contains(closedViewInfo.View));
                Assert.IsTrue(_navigationService.NbOpenedViews == 2);
            }

            [TestMethod]
            public void CancelTheCloseApplicationControlOnModalViewRestoresCorrectModalStates()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey", new FakeDirtyViewModel());
                var parentViewInfo = _navigationService.NavigateTo<UserControl>(navigationInfo);
                _navigationService.ShowMessageBox("viewKey", "Hello");
                _navigationService.CloseApplication();
                var closeApplicationControl = (ShutdownApplicationControl)((ModalHostControl)_navigationService.CurrentView.View).ModalContent;

                // Act
                _navigationService.Close(closeApplicationControl.ViewKey);  // simulates a click on 'Cancel' button

                // Verify
                Assert.IsTrue(_navigationService.CurrentView.View.Visibility == Visibility.Visible);
                Assert.IsTrue(_navigationService.OpenedViews.First(v => v == parentViewInfo).View.Visibility == Visibility.Visible);
                Assert.IsFalse(_navigationService.OpenedViews.First(v => v == parentViewInfo).View.IsEnabled );
            }

            [TestMethod]
            public void ClosingActiveAwareViewSupported()
            {
                // Prepare
                var activeAwareViewModelMock = new Mock<IActiveAware>();
                var navigationInfo = NavigationInfo.CreateSimple("viewKey", activeAwareViewModelMock.Object);
                _navigationService.NavigateTo<UserControl>(navigationInfo);

                // Act
                _navigationService.Close("viewKey");

                // Verify
                activeAwareViewModelMock.Verify(m => m.OnDeactivating(), Times.Exactly(1));
                activeAwareViewModelMock.Verify(m => m.OnDeactivated(), Times.Exactly(1));
            }
        }

        [TestClass]
        public class CloseApplication : NavigationServiceFixture
        {
            [TestMethod]
            public void CanShowCloseableViewSelection()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateComplex("viewKey2", navigationInfo1.ViewKey, new FakeDirtyViewModel());
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Act
                _navigationService.CloseApplication();

                // Verify
                Assert.IsInstanceOfType(_navigationService.CurrentView.View, typeof(ModalHostControl));
                Assert.IsInstanceOfType(((ModalHostControl)_navigationService.CurrentView.View).ModalContent, typeof(ShutdownApplicationControl));
            }

            [TestMethod]
            public void OnlyTopMostAndCloseableViewsAreProposed()
            {
                // Prepare
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateComplex("viewKey2", navigationInfo1.ViewKey, new FakeDirtyViewModel());
                var navigationInfo3 = NavigationInfo.CreateSimple("viewKey3", new FakeDirtyViewModel());
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                var expectedViewInfo1 = _navigationService.NavigateTo<UserControl>(navigationInfo2);
                var expectedViewInfo2 = _navigationService.NavigateTo<UserControl>(navigationInfo3);

                // Act
                _navigationService.CloseApplication();

                // Verify
                var closeApplicationControl = (ShutdownApplicationControl)((ModalHostControl)_navigationService.CurrentView.View).ModalContent;
                Assert.IsTrue(closeApplicationControl.NbCloseableViews == 2);
                Assert.IsTrue(closeApplicationControl.ItemsSource.Cast<ViewInfo>().ToList().Contains(expectedViewInfo1));
                Assert.IsTrue(closeApplicationControl.ItemsSource.Cast<ViewInfo>().Contains(expectedViewInfo2));
            }

            [TestMethod]
            public void MessageBoxesAreNotConsideredAsTopMostViewsDuringClosingProcess()
            {
                // Prepare
                var navigationInfo = NavigationInfo.CreateSimple("viewKey", new FakeDirtyViewModel());
                _navigationService.NavigateTo<UserControl>(navigationInfo);
                _navigationService.ShowMessageBox("viewKey", "Hello");

                // Act
                _navigationService.CloseApplication();

                // Verify
                Assert.IsInstanceOfType(_navigationService.CurrentView.View, typeof(ModalHostControl));
                Assert.IsInstanceOfType(((ModalHostControl)_navigationService.CurrentView.View).ModalContent, typeof(ShutdownApplicationControl));
            }

            [TestMethod]
            public void CanRaiseClosingApplicationShownEvent()
            {
                // Prepare
                var eventRaised = false;
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateComplex("viewKey2", navigationInfo1.ViewKey, new FakeDirtyViewModel());
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Act
                _navigationService.ShutdownApplicationShown += (sender, e) => eventRaised = true;
                _navigationService.CloseApplication();

                // Verify
                Assert.IsTrue(eventRaised);
            }

            [TestMethod]
            public void CanRaiseClosingApplicationHiddenEvent()
            {
                // Prepare
                var eventRaised = false;
                var navigationInfo1 = NavigationInfo.CreateSimple("viewKey1");
                var navigationInfo2 = NavigationInfo.CreateComplex("viewKey2", navigationInfo1.ViewKey, new FakeDirtyViewModel());
                _navigationService.NavigateTo<UserControl>(navigationInfo1);
                _navigationService.NavigateTo<UserControl>(navigationInfo2);

                // Act
                _navigationService.ShutdownApplicationHidden += (sender, e) => eventRaised = true;
                _navigationService.CloseApplication();
                var closingApplicationViewKey = _navigationService.CurrentView.ViewKey;
                _navigationService.Close(closingApplicationViewKey);

                // Verify
                Assert.IsTrue(eventRaised);
            }
        }
    }
}