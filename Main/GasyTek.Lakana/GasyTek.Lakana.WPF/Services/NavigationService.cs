using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.WPF.Controls;

namespace GasyTek.Lakana.WPF.Services
{
    public class NavigationService : INavigationService
    {
        #region Events

        public event EventHandler ClosingApplicationShown;
        public event EventHandler ClosingApplicationHidden;

        #endregion

        #region Fields

        private Panel _rootPanel;
        private AnimateTransitionAction _animateTransitionAction;
        private readonly LinkedList<LinkedList<ViewInfo>> _viewCollection;
        private readonly ReadOnlyObservableCollection<ViewInfo> _readonlyOpenedViews;
        private readonly ObservableCollection<ViewInfo> _openedViews;

        #endregion

        #region Properties

        public Panel RootPanel
        {
            get { return _rootPanel; }
        }

        public ViewInfo CurrentView
        {
            get
            {
                var currentView = GetCurrentView();
                return currentView != null ? currentView.Value : ViewInfo.Null;
            }
        }

        public ReadOnlyObservableCollection<ViewInfo> OpenedViews
        {
            get { return _readonlyOpenedViews; }
        }

        public int NbOpenedViews
        {
            get { return _viewCollection.Sum(vs => vs.Count); }
        }

        #endregion

        #region Constructor

        public NavigationService()
        {
            _viewCollection = new LinkedList<LinkedList<ViewInfo>>();
            _openedViews = new ObservableCollection<ViewInfo>();
            _readonlyOpenedViews = new ReadOnlyObservableCollection<ViewInfo>(_openedViews);
        }

        #endregion

        public void CreateWorkspace(Panel rootPanel, AnimateTransitionAction animateTransitionAction)
        {
            _rootPanel = rootPanel;
            _animateTransitionAction = animateTransitionAction;
        }

        public ViewInfo NavigateTo<TView>(NavigationInfo navigationInfo) where TView : FrameworkElement, new()
        {
            // Creates a new instance of the view
            var view = new TView();

            return navigationInfo.HasParentKey ?
                NavigateToInternal(navigationInfo.ViewKey, navigationInfo.ParentViewKey, view, navigationInfo.ViewModel, false, navigationInfo.IsOpenedViewMember)
                : NavigateToInternal(navigationInfo.ViewKey, view, navigationInfo.ViewModel, navigationInfo.IsOpenedViewMember);
        }

        private ViewInfo NavigateToInternal(string viewKey, string parentViewKey, FrameworkElement view, object viewModel, bool isModal, bool isOpenedViewMember)
        {
            ViewInfo resultViewInfo;
            LinkedListNode<ViewInfo> foundChildViewInfoNode;

            var oldView = CurrentView;

            if (TryFindViewInternal(viewKey, out foundChildViewInfoNode))
            {
                if (foundChildViewInfoNode.Previous == null || foundChildViewInfoNode.Previous.Value != new ViewInfo(parentViewKey))
                    throw new ChildViewAlreadyExistsException(viewKey, parentViewKey);
                
                // If the combination (ViewKey, ParentViewKey) already exists then make their stack the current one
                _viewCollection.Remove(foundChildViewInfoNode.List);
                _viewCollection.AddLast(foundChildViewInfoNode.List);
                resultViewInfo = foundChildViewInfoNode.Value;
            }
            else
            {
                var foundParentViewInfoNode = FindViewInternal(parentViewKey);

                if (!IsTopMostView(foundParentViewInfoNode))
                    throw new ParentViewNotTopMostException(parentViewKey);

                resultViewInfo = new ViewInfo(viewKey) { View = view, IsModal = isModal, IsOpenedViewMember = isOpenedViewMember };

                EnforceViewKey(view, viewModel, viewKey);
                EnforceUIMetadata(view, viewModel, ref resultViewInfo);

                // Link view and view model
                view.DataContext = viewModel;
                
                foundParentViewInfoNode.List.AddLast(resultViewInfo);

                AddOpenedView(resultViewInfo);
            }

            PerformTransitionAnimation(RootPanel, oldView.View, CurrentView.View, resultViewInfo.IsModal);

            return resultViewInfo;
        }

        private ViewInfo NavigateToInternal(string viewKey, FrameworkElement view, object viewModel, bool isOpenedViewMember)
        {
            ViewInfo resultViewInfo;
            LinkedListNode<ViewInfo> foundViewInfoNode;

            var oldView = CurrentView;

            if (TryFindViewInternal(viewKey, out foundViewInfoNode))
            {
                // The view already exists
                _viewCollection.Remove(foundViewInfoNode.List);
                _viewCollection.AddLast(foundViewInfoNode.List);
                resultViewInfo = foundViewInfoNode.Value;
            }
            else
            {
                resultViewInfo = new ViewInfo(viewKey) { View = view, IsOpenedViewMember = isOpenedViewMember };

                EnforceViewKey(view, viewModel, viewKey);
                EnforceUIMetadata(view, viewModel, ref resultViewInfo);

                // Link view and view model
                view.DataContext = viewModel;

                var viewStack = new LinkedList<ViewInfo>();
                viewStack.AddLast(resultViewInfo);

                _viewCollection.AddLast(viewStack);
                AddOpenedView(resultViewInfo);
            }

            PerformTransitionAnimation(RootPanel, oldView.View, CurrentView.View, CurrentView.IsModal);

            return resultViewInfo;
        }

        public ViewInfo NavigateTo(string viewKey)
        {
            var oldView = CurrentView;
            var foundViewInfoNode = FindViewInternal(viewKey);

            // moves the stack to the last position
            _viewCollection.Remove(foundViewInfoNode.List);
            _viewCollection.AddLast(foundViewInfoNode.List);

            PerformTransitionAnimation(RootPanel, oldView.View, CurrentView.View, CurrentView.IsModal);

            return foundViewInfoNode.Value;
        }

        public ModalResult<TResult> ShowModal<TView, TResult>(NavigationInfo navigationInfo) where TView : FrameworkElement, new()
        {
            if (!navigationInfo.HasParentKey)
                throw new InvalidOperationException("Parent view key must be initialized");

            return ShowModalInternal<TResult>(new TView(), navigationInfo.ViewKey, navigationInfo.ParentViewKey, navigationInfo.ViewModel, navigationInfo.IsOpenedViewMember);
        }

        public Task<MessageBoxResult> ShowMessageBox(string parentViewKey, string message = "", MessageBoxImage messageBoxImage = MessageBoxImage.Information, MessageBoxButton messageBoxButton = MessageBoxButton.OK)
        {
            var messageBoxViewKey = Guid.NewGuid().ToString();
            var view = new MessageBoxControl
            {
                NavigationService = this,
                ViewKey = messageBoxViewKey,
                Message = message,
                MessageBoxImage = messageBoxImage,
                MessageBoxButton = messageBoxButton
            };

            var modalResult = ShowModalInternal<MessageBoxResult>(view, messageBoxViewKey, parentViewKey, null, false);

            return modalResult.Result;
        }

        private ModalResult<TResult> ShowModalInternal<TResult>(FrameworkElement view, string viewKey, string parentViewKey, object viewModel, bool isOpenedViewMember)
        {
            var modalHostControl = new ModalHostControl { ModalContent = view };
            var viewInfo = NavigateToInternal(viewKey, parentViewKey, modalHostControl, viewModel, true, isOpenedViewMember);

            return new ModalResult<TResult> (modalHostControl.ResultCompletionSource.Task) { ViewInfo = viewInfo };
        }

        public ViewInfo Close(string viewKey, object modalResult = null)
        {
            var foundViewInfoNode = FindViewInternal(viewKey);

            if (!IsTopMostView(foundViewInfoNode))
                throw new ParentViewNotTopMostException(viewKey);

            var stack = foundViewInfoNode.List;
            var closedView = stack.Last.Value;
            stack.RemoveLast();

            if (stack.Count == 0) _viewCollection.Remove(stack);

            // Raise ClosingApplicationHidden event if the view to close is the closing application view
            if (foundViewInfoNode.Value.View is ModalHostControl 
                && ((ModalHostControl)foundViewInfoNode.Value.View).ModalContent is CloseApplicationControl 
                && ClosingApplicationHidden != null)
                    ClosingApplicationHidden(this, new EventArgs());

            // Manage modal views
            if (foundViewInfoNode.Value.IsModal && foundViewInfoNode.Value.View is ModalHostControl)
            {
                var modalHostControl = (ModalHostControl)foundViewInfoNode.Value.View;
                modalHostControl.ResultCompletionSource.SetResult(modalResult);
            }

            PerformTransitionAnimation(RootPanel, closedView.View, CurrentView.View);
            RemoveOpenedView(closedView);

            return closedView;
        }

        public bool CloseApplication(bool forceClose = false)
        {
            // exit the application without any security
            if (forceClose)
            {
                OnCloseApplicationExecuted();
                return true;
            }

            // if there is no views or view models that prevent the app from closing
            var notCloseableViews = GetNotCloseableViews().ToList();
            if (notCloseableViews.Any() == false)
            {
                OnCloseApplicationExecuted();
                return true;
            }

            var viewKey = Guid.NewGuid().ToString();
            var closeApplicationView = new CloseApplicationControl
                                              {
                                                  ItemsSource = notCloseableViews.OrderBy(v => v.ViewKey),
                                                 NavigationService = this,
                                                 ViewKey = viewKey
                                              };

            ShowModalInternal<object>(closeApplicationView, viewKey, CurrentView.ViewKey, null, false);

            if(ClosingApplicationShown != null)
                ClosingApplicationShown(this, new EventArgs());

            return false;
        }

        #region Overridable methods

        protected virtual void OnCloseApplicationExecuted()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Private methods

        private LinkedListNode<ViewInfo> GetCurrentView()
        {
            if (!_viewCollection.Any()) return null;
            if (!_viewCollection.Last.Value.Any()) return null;
            return _viewCollection.Last.Value.Last;
        }

        private IEnumerable<ViewInfo> GetNotCloseableViews()
        {
            return  from vs in _viewCollection
                    let v = vs.Last.Value.View as ICloseable
                    let vm = vs.Last.Value.View.DataContext as ICloseable
                    where (v != null && v.CanClose() == false) || (vm != null && vm.CanClose() == false)
                    select vs.Last.Value;
        }

        private void PerformTransitionAnimation(Panel rootPanel, FrameworkElement oldView, FrameworkElement newView, bool isModal = false)
        {
            EnsureViewsAreDisplayed(oldView, newView);

            if (_animateTransitionAction != null)
            {
                _animateTransitionAction(oldView, newView);
            }

            foreach (UIElement child in rootPanel.Children)
            {
                if (ReferenceEquals(child, newView)) continue;

                Panel.SetZIndex(child, 0);
                child.Visibility = Visibility.Collapsed;
                child.IsEnabled = true;
            }

            // manage modal views
            if (isModal)
            {
                var currentView = GetCurrentView();
                if (currentView.Previous != null)
                {
                    var parentViewInfo = currentView.Previous.Value;
                    parentViewInfo.View.Visibility = Visibility.Visible;
                    parentViewInfo.View.IsEnabled = false;
                }
            }
        }

        private void EnsureViewsAreDisplayed(FrameworkElement oldView, FrameworkElement newView)
        {
            // display the old and new view
            if (oldView != null)
            {
                Panel.SetZIndex(oldView, 100);
                oldView.Visibility = Visibility.Visible;
                oldView.IsEnabled = true;
            }

            if (newView != null)
            {
                Panel.SetZIndex(newView, 0);
                newView.Visibility = Visibility.Visible;
                newView.IsEnabled = true;
            }
        }

        private void AddOpenedView(ViewInfo viewInfo)
        {
            RootPanel.Children.Add(viewInfo.View);

            if (viewInfo.IsOpenedViewMember)
                _openedViews.Add(viewInfo);
        }

        private void RemoveOpenedView(ViewInfo viewInfo)
        {
            RootPanel.Children.Remove(viewInfo.View);

            if (viewInfo.IsOpenedViewMember)
                _openedViews.Remove(viewInfo);
        }

        private LinkedListNode<ViewInfo> FindViewInternal(string viewkey)
        {
            var viewInfo = new ViewInfo(viewkey);
            var q = from stack in _viewCollection
                    from v in stack
                    where v == viewInfo
                    select stack.Find(v);
            if (q.Any()) return q.First();
            throw new ViewNotFoundException(viewkey);
        }

        private bool TryFindViewInternal(string viewkey, out LinkedListNode<ViewInfo> result)
        {
            try
            {
                result = FindViewInternal(viewkey);
                return true;
            }
            catch (ViewNotFoundException)
            {
                result = null;
                return false;
            }
        }

        private bool IsTopMostView(LinkedListNode<ViewInfo> viewInfo)
        {
            return viewInfo.List.Last.Value == viewInfo.Value;
        }

        private void EnforceViewKey(FrameworkElement view, object viewModel, string viewKey)
        {
            // Initializes view key on the view 
            var viewKeyAwareView = view as IViewKeyAware;
            if (viewKeyAwareView != null)
                viewKeyAwareView.ViewKey = viewKey;

            // Initializes view key on the view model 
            var viewKeyAwareViewModel = viewModel as IViewKeyAware;
            if (viewKeyAwareViewModel != null)
                viewKeyAwareViewModel.ViewKey = viewKey;
        }

        private void EnforceUIMetadata(FrameworkElement view, object viewModel, ref ViewInfo targetViewInfo)
        {                
            // Extract presentation metadata from view first if possible
            var presentableView = view as IPresentable;
            if (presentableView != null)
                targetViewInfo.UIMetadata = presentableView.UIMetadata;

            // Extract presentation metadata from view model if possible
            var presentableViewModel = viewModel as IPresentable;
            if (presentableViewModel != null)
                targetViewInfo.UIMetadata = presentableViewModel.UIMetadata;
        }

        #endregion
    }
}
