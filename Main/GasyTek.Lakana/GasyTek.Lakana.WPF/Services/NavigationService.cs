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
                NavigateToInternal(navigationInfo.ViewKey, navigationInfo.ParentViewKey, view, navigationInfo.ViewContext, false, navigationInfo.IsOpenedView)
                : NavigateToInternal(navigationInfo.ViewKey, view, navigationInfo.ViewContext, navigationInfo.IsOpenedView);
        }

        private ViewInfo NavigateToInternal(string viewKey, string parentViewKey, FrameworkElement view, object viewContext, bool isModal, bool isOpenedView)
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

                resultViewInfo = new ViewInfo(viewKey) { View = view, IsModal = isModal, IsOpenedView = isOpenedView };

                // Initializes view key on the view model 
                var viewKeyAwareViewContext = viewContext as IViewKeyAware;
                if (viewKeyAwareViewContext != null)
                    viewKeyAwareViewContext.ViewKey = viewKey;

                // Extract presentation metadata from view first if possible
                var presentableView = view as IPresentable;
                if (presentableView != null)
                    resultViewInfo.PresentationMetadata = presentableView.PresentationMetadata;

                // Extract presentation metadata from view model if possible
                var presentableViewContext = viewContext as IPresentable;
                if (presentableViewContext != null)
                    resultViewInfo.PresentationMetadata = presentableViewContext.PresentationMetadata;

                // Link view and view model
                view.DataContext = viewContext;
                
                foundParentViewInfoNode.List.AddLast(resultViewInfo);

                AddOpenedView(resultViewInfo);
            }

            PerformTransitionAnimation(RootPanel, oldView.View, CurrentView.View, resultViewInfo.IsModal);

            return resultViewInfo;
        }

        private ViewInfo NavigateToInternal(string viewKey, FrameworkElement view, object viewContext, bool isOpenedView)
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
                resultViewInfo = new ViewInfo(viewKey) { View = view, IsOpenedView = isOpenedView };

                // Initializes view key on the view model 
                var viewKeyAwareViewContext = viewContext as IViewKeyAware;
                if (viewKeyAwareViewContext != null)
                    viewKeyAwareViewContext.ViewKey = viewKey;

                // Extract presentation metadata from view first if possible
                var presentableView = view as IPresentable;
                if (presentableView != null)
                    resultViewInfo.PresentationMetadata = presentableView.PresentationMetadata;

                // Extract presentation metadata from view model if possible
                var presentableViewContext = viewContext as IPresentable;
                if (presentableViewContext != null)
                    resultViewInfo.PresentationMetadata = presentableViewContext.PresentationMetadata;

                // Link view and view model
                view.DataContext = viewContext;

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

        public ViewInfo ShowModal<TView>(NavigationInfo navigationInfo) where TView : FrameworkElement, new()
        {
            if (!navigationInfo.HasParentKey)
                throw new InvalidOperationException("Parent view key must be initialized");

            var modalHostControl = new ModalHostControl { ModalContent = new TView() };

            return NavigateToInternal(navigationInfo.ViewKey, navigationInfo.ParentViewKey, modalHostControl,
                             navigationInfo.ViewContext, true, navigationInfo.IsOpenedView);
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
            NavigateToInternal(messageBoxViewKey, parentViewKey, view, null, true, false);
            return view.Result;
        }

        public ViewInfo Close(string viewKey)
        {
            var foundViewInfoNode = FindViewInternal(viewKey);

            if (!IsTopMostView(foundViewInfoNode))
                throw new ParentViewNotTopMostException(viewKey);

            var stack = foundViewInfoNode.List;
            var closedView = stack.Last.Value;
            stack.RemoveLast();

            if (stack.Count == 0) _viewCollection.Remove(stack);

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

            var closeableViews = GetCloseableViews().ToList();

            if (!closeableViews.Any())
            {
                OnCloseApplicationExecuted();
                return true;
            }

            var viewKey = Guid.NewGuid().ToString();
            var closeApplicationControl = new CloseApplicationControl
                                              {
                                                  ItemsSource = closeableViews.OrderBy(v => v.ViewKey),
                                                 NavigationService = this,
                                                 ViewKey = viewKey
                                              };

            NavigateToInternal(viewKey, CurrentView.ViewKey, closeApplicationControl, null, true, false);
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

        private IEnumerable<ViewInfo> GetCloseableViews()
        {
            return  from vs in _viewCollection
                    let icl = vs.Last.Value.View.DataContext as ICloseable
                    where icl != null && !icl.CanClose()
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

            if (viewInfo.IsOpenedView)
                _openedViews.Add(viewInfo);
        }

        private void RemoveOpenedView(ViewInfo viewInfo)
        {
            RootPanel.Children.Remove(viewInfo.View);

            if (viewInfo.IsOpenedView)
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

        #endregion
    }
}
