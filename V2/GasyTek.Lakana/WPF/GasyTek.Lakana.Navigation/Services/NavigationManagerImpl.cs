using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Adapters;
using GasyTek.Lakana.Navigation.Controls;
using GasyTek.Lakana.Navigation.Transitions;

namespace GasyTek.Lakana.Navigation.Services
{
    internal class NavigationManagerImpl : INavigationManager
    {
        #region Fields

        private readonly IViewLocator _viewLocator;
        private readonly ViewGroupCollectionManager _viewGroupCollectionManager;
        private IWorkspaceAdapter _workspaceAdapter;

        #endregion

        #region Properties

        internal ViewGroupCollectionManager ViewGroupCollectionManager
        {
            get { return _viewGroupCollectionManager; }
        }

        public View ActiveView
        {
            get
            {
                var activeNode = ActiveNode;
                return activeNode != null ? activeNode.Value : View.Null;
            }
        }

        internal ViewGroupNode ActiveNode
        {
            get { return _viewGroupCollectionManager.GetActiveNode(); }
        }

        internal int NbViews
        {
            get { return _viewGroupCollectionManager.NbViews; }
        }

        #endregion

        #region Constructor

        public NavigationManagerImpl(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
            _viewGroupCollectionManager = new ViewGroupCollectionManager();
        }

        #endregion

        #region Public methods

        public NavigationResult NavigateTo(string navigationKey, object viewModel)
        {
            return NavigateToInternal(navigationKey, viewModel, false, false);
        }

        public NavigationResult NavigateTo(string navigationKey)
        {
            return NavigateTo(navigationKey, null);
        }

        public ModalResult<TResult> ShowModal<TResult>(string navigationKey, object viewModel)
        {
            NavigationKey.EnsuresNavigationKeyHasParent(navigationKey);

            var nResult = NavigateToInternal(navigationKey, viewModel, true, false);
            var viewInfo = nResult.View;
            var modalHostControl = (ModalHostControl)viewInfo.ViewHostInstance.View;

            return new ModalResult<TResult>(modalHostControl.ResultCompletionSource.Task, viewInfo);
        }

        public ModalResult<TResult> ShowModal<TResult>(string navigationKey)
        {
            return ShowModal<TResult>(navigationKey, null);
        }

        public Task<MessageBoxResult> ShowMessageBox(string ownerViewKey, string message = "", MessageBoxImage messageBoxImage = MessageBoxImage.Information, MessageBoxButton messageBoxButton = MessageBoxButton.OK)
        {
            var modalViewInstanceKey = Guid.NewGuid().ToString("N");
            var navigationKey = string.Format("{0}/{1}", ownerViewKey, modalViewInstanceKey);
            var nResult = NavigateToInternal(navigationKey, null, true, true);

            var viewInfo = nResult.View;

            // initialize messagebox informations
            var messageBoxControl = (MessageBoxControl)viewInfo.ViewInstance;
            messageBoxControl.ViewInstanceKey = modalViewInstanceKey;
            messageBoxControl.NavigationManager = this;
            messageBoxControl.Message = message;
            messageBoxControl.MessageBoxImage = messageBoxImage;
            messageBoxControl.MessageBoxButton = messageBoxButton;

            var modalHostControl = (ModalHostControl)viewInfo.ViewHostInstance.View;
            return modalHostControl.ResultCompletionSource.Task
                .ContinueWith(t => (MessageBoxResult)t.Result);
        }

        public NavigationResult Close(string viewKey, object modalResult = null)
        {
            if (!_viewGroupCollectionManager.IsTopMostView(viewKey))
                throw new CannotCloseNotTopMostViewException(viewKey);

            var closedNode = _viewGroupCollectionManager.RemoveViewNode(viewKey);

            // perform update of the UI
            var newNode = _viewGroupCollectionManager.GetActiveNode();
            var oldNode = closedNode;

            if (closedNode.Value.IsModal)
            {
                var modalHostControl = (ModalHostControl)closedNode.Value.ViewHostInstance.View;
                modalHostControl.ResultCompletionSource.SetResult(modalResult);
            }

            var asyncTransition = _workspaceAdapter.PerformUIClose(oldNode, newNode);

            return new NavigationResult(asyncTransition, closedNode.Value);
        }

        public bool CloseApplication(bool forceClose = false)
        {
            // exit the application as is
            if (forceClose)
            {
                OnCloseApplicationExecuted();
                return true;
            }

            // if there is no views or view models that prevent the app from closing, then close
            var notCloseableViews = _viewGroupCollectionManager.GetNotCloseableViews().ToList();
            if (notCloseableViews.Any() == false)
            {
                OnCloseApplicationExecuted();
                return true;
            }

            // display shutdown application dialog
            var shutdownApplicationWindow = new ShutdownApplicationWindow
            {
                Owner = Application.Current != null ? Application.Current.MainWindow : null,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Views = new ObservableCollection<View>(notCloseableViews.OrderBy(v => v.ViewInstanceKey).ToList()),
                NavigationManager = this
            };

            var dialogResult = shutdownApplicationWindow.ShowDialog();
            return dialogResult != null && dialogResult.Value;
        }

        private NavigationResult NavigateToInternal(string navigationKey, object viewModel, bool isModal, bool isMessageBox)
        {
            var navigationKeyInstance = NavigationKey.Parse(navigationKey);
            var parentViewInstanceKey = navigationKeyInstance.ParentViewInstanceKey;
            var viewInstanceKey = navigationKeyInstance.ViewInstanceKey;
            var viewKey = navigationKeyInstance.ViewKey;
            var hasParent = navigationKeyInstance.HasParent;
            var oldNode = _viewGroupCollectionManager.GetActiveNode();

            ViewGroupNode newNode;

            // if the navigation key refers to a parent e.g : "parentView/view"

            if (hasParent)
            {
                if (_viewGroupCollectionManager.TryFindViewNode(viewInstanceKey, out newNode))
                {
                    // activates an existing node
                    EnsuresViewHasParentAndMatch(newNode, parentViewInstanceKey);
                    _viewGroupCollectionManager.ActivateExistingNode(newNode);
                }
                else
                {
                    // activates a new node
                    EnsureParentViewExist(parentViewInstanceKey);
                    EnsureParentViewIsTopMost(parentViewInstanceKey);

                    newNode = CreateNewNodeFrom(viewInstanceKey, viewKey, viewModel, isModal, isMessageBox);

                    var parentNode = _viewGroupCollectionManager.FindViewNode(parentViewInstanceKey);
                    _viewGroupCollectionManager.ActivateNewNode(newNode, parentNode.List);
                }

                // perform update of the UI
                var asyncTransition1 = _workspaceAdapter.PerformUIActivation(oldNode, newNode);

                return new NavigationResult(asyncTransition1, newNode.Value);
            }

            // if the navigation key has a simple form e.g : "view"

            if (_viewGroupCollectionManager.TryFindViewNode(viewInstanceKey, out newNode))
            {
                // activates an existing node
                newNode = _viewGroupCollectionManager.IsTopMostView(viewInstanceKey) ? newNode : newNode.List.Peek();
                _viewGroupCollectionManager.ActivateExistingNode(newNode);
            }
            else
            {
                // activates a new node
                newNode = CreateNewNodeFrom(viewInstanceKey, viewKey, viewModel, isModal, isMessageBox);
                _viewGroupCollectionManager.ActivateNewNode(newNode);
            }

            // perform update of the UI
            var asyncTransition2 = _workspaceAdapter.PerformUIActivation(oldNode, newNode);

            return new NavigationResult(asyncTransition2, newNode.Value);
        }

        #endregion

        #region Internal methods

        internal void SetMainWorkspace(Panel mainWorkspace, IWorkspaceAdapter workspaceAdapter, Func<TransitionAnimation> transitionAnimationsProvider = null)
        {
            if (workspaceAdapter == null)
                throw new ArgumentNullException("workspaceAdapter");

            _workspaceAdapter = workspaceAdapter;
            _workspaceAdapter.SetMainWorkspace(mainWorkspace);
            _workspaceAdapter.SetViewGroupCollection(_viewGroupCollectionManager.ViewGroupCollection);
            _workspaceAdapter.SetTransitionAnimationProvider(transitionAnimationsProvider);

            _viewLocator.RegisterApplicationViews();
        }

        #endregion

        #region Overridable methods

        protected virtual void OnCloseApplicationExecuted()
        {
            if (Application.Current != null)
                Application.Current.Shutdown();
        }

        #endregion

        #region Private methods

        private ViewGroupNode CreateNewNodeFrom(string viewInstanceKey, string viewKey, object viewModel, bool isModal, bool isMessageBox)
        {
            FrameworkElement viewInstance;

            if (isMessageBox) viewInstance = new MessageBoxControl();
            else
            {
                viewInstance = _viewLocator.GetViewInstance(viewKey);
                viewInstance.DataContext = viewModel;

                EnforceViewKey(viewInstance, viewModel, viewInstanceKey);
            }

            var internalViewInstance = viewInstance;

            if (isModal)
            {
                var modalHost = new ModalHostControl { ModalContent = viewInstance };
                internalViewInstance = modalHost;
            }

            var viewInfo = new View(viewInstanceKey)
                       {
                           UIMetadata = GetUIMetadata(viewInstance, viewModel),
                           ViewHostInstance = new ViewHostControl { View = internalViewInstance },
                           ViewInstance = viewInstance,
                           ViewModelInstance = viewModel,
                           IsModal = isModal,
                           IsMessageBox = isMessageBox
                       };

            return new ViewGroupNode(null, viewInfo);
        }

        private IUIMetadata GetUIMetadata(object view, object viewModel)
        {
            IUIMetadata uiMetadata = null;

            var presentable = view as IPresentable;
            if (presentable != null)
            {
                uiMetadata = presentable.UIMetadata;
            }

            presentable = viewModel as IPresentable;
            if (presentable != null)
            {
                uiMetadata = presentable.UIMetadata;
            }

            return uiMetadata;
        }

        private void EnsuresViewHasParentAndMatch(ViewGroupNode node, string expectedParentViewInstanceKey)
        {
            var expectedParentView = new View(expectedParentViewInstanceKey);
            if (node.Previous == null || (node.Previous != null && node.Previous.Value != expectedParentView))
            {
                throw new OnlyNewViewInstanceCanBeStackedException(node.Value.ViewInstanceKey, expectedParentViewInstanceKey);
            }
        }

        private void EnsureParentViewIsTopMost(string parentViewInstanceKey)
        {
            if (_viewGroupCollectionManager.IsTopMostView(parentViewInstanceKey) == false)
                throw new ParentViewInstanceNotTopMostException(parentViewInstanceKey);
        }

        private void EnsureParentViewExist(string parentViewInstanceKey)
        {
            if (_viewGroupCollectionManager.ContainsViewNode(parentViewInstanceKey) == false)
                throw new ParentViewInstanceNotFoundException(parentViewInstanceKey);
        }

        #endregion

        #region IViewKeyAware helper

        private void EnforceViewKey(FrameworkElement view, object viewModel, string viewInstanceKey)
        {
            // Initializes view key on the view 
            var viewKeyAwareView = view as IViewKeyAware;
            if (viewKeyAwareView != null)
                viewKeyAwareView.ViewInstanceKey = viewInstanceKey;

            // Initializes view key on the view model 
            var viewKeyAwareViewModel = viewModel as IViewKeyAware;
            if (viewKeyAwareViewModel != null)
                viewKeyAwareViewModel.ViewInstanceKey = viewInstanceKey;
        }

        #endregion

        #region Private class NavigationKey

        private class NavigationKey
        {
            #region Patterns

            private const string Pattern1 = @"\A([0-9a-zA-Z]+)\Z";                                                   // e.g : view1
            private const string Pattern2 = @"\A([0-9a-zA-Z]+\#[0-9a-zA-Z]+)\Z";                                     // e.g : view1#abc1
            private const string Pattern3 = @"\A(([0-9a-zA-Z]+)/([0-9a-zA-Z]+))\Z";                                  // e.g : parentView1/view1
            private const string Pattern4 = @"\A(([0-9a-zA-Z]+\#[0-9a-zA-Z]+)/([0-9a-zA-Z]+))\Z";                    // e.g : parentView1#abc1/view1   
            private const string Pattern5 = @"\A(([0-9a-zA-Z]+)/([0-9a-zA-Z]+\#[0-9a-zA-Z]+))\Z";                    // e.g : parentView1/view1#abc1   
            private const string Pattern6 = @"\A(([0-9a-zA-Z]+\#[0-9a-zA-Z]+)/([0-9a-zA-Z]+\#[0-9a-zA-Z]+))\Z";      // e.g : parentView1#abc1/view1#abc1

            #endregion

            internal string ViewInstanceKey { get; private set; }
            internal string ViewKey { get; private set; }

            internal string ParentViewInstanceKey { get; private set; }
            internal string ParentViewKey { get; set; }

            internal bool HasParent
            {
                get { return !String.IsNullOrEmpty(ParentViewInstanceKey); }
            }

            private NavigationKey() { }

            internal static NavigationKey Parse(string navigationKey)
            {
                if (Regex.IsMatch(navigationKey, Pattern1))
                {
                    // view1
                    var viewInstanceKey = navigationKey;
                    return new NavigationKey { ViewInstanceKey = viewInstanceKey, ViewKey = viewInstanceKey };
                }

                if (Regex.IsMatch(navigationKey, Pattern2))
                {
                    // view1#abc1
                    var parts = navigationKey.Split('#');
                    var viewInstanceKey = navigationKey;
                    var viewKey = parts[0];
                    return new NavigationKey { ViewInstanceKey = viewInstanceKey, ViewKey = viewKey };

                }

                if (Regex.IsMatch(navigationKey, Pattern3))
                {
                    // parentView1/view1
                    var parts = navigationKey.Split('/');
                    var parentViewInstanceKey = parts[0];
                    var viewInstanceKey = parts[1];
                    return new NavigationKey
                               {
                                   ParentViewInstanceKey = parentViewInstanceKey,
                                   ParentViewKey = parentViewInstanceKey,
                                   ViewInstanceKey = viewInstanceKey,
                                   ViewKey = viewInstanceKey
                               };
                }

                if (Regex.IsMatch(navigationKey, Pattern4))
                {
                    // parentView1#abc1/view1 
                    var parts = navigationKey.Split('/');
                    var parentViewInstanceKey = parts[0];
                    var parentViewKey = parentViewInstanceKey.Split('#')[0];
                    var viewInstanceKey = parts[1];
                    return new NavigationKey
                               {
                                   ParentViewInstanceKey = parentViewInstanceKey,
                                   ParentViewKey = parentViewKey,
                                   ViewInstanceKey = viewInstanceKey,
                                   ViewKey = viewInstanceKey
                               };
                }

                if (Regex.IsMatch(navigationKey, Pattern5))
                {
                    // parentView1/view1#abc1 
                    var parts = navigationKey.Split('/');
                    var parentViewInstanceKey = parts[0];
                    var viewInstanceKey = parts[1];
                    var viewKey = viewInstanceKey.Split('#')[0];
                    return new NavigationKey
                                {
                                    ParentViewInstanceKey = parentViewInstanceKey,
                                    ParentViewKey = parentViewInstanceKey,
                                    ViewInstanceKey = viewInstanceKey,
                                    ViewKey = viewKey
                                };
                }

                if (Regex.IsMatch(navigationKey, Pattern6))
                {
                    // parentView1#abc1/view1#abc1
                    var parts = navigationKey.Split('/');
                    var parentViewInstanceKey = parts[0];
                    var parentViewKey = parentViewInstanceKey.Split('#')[0];
                    var viewInstanceKey = parts[1];
                    var viewKey = viewInstanceKey.Split('#')[0];
                    return new NavigationKey
                                {
                                    ParentViewInstanceKey = parentViewInstanceKey,
                                    ParentViewKey = parentViewKey,
                                    ViewInstanceKey = viewInstanceKey,
                                    ViewKey = viewKey
                                };
                }

                throw new NavigationKeyFormatException("Allowed format is '[ parentViewKey [ # instanceID ] / ] viewKey [ # instanceID ]'");
            }

            internal static void EnsuresNavigationKeyHasParent(string navigationKey)
            {
                if (Regex.IsMatch(navigationKey, Pattern3)) return;
                if (Regex.IsMatch(navigationKey, Pattern4)) return;
                if (Regex.IsMatch(navigationKey, Pattern5)) return;
                if (Regex.IsMatch(navigationKey, Pattern6)) return;

                throw new NavigationKeyFormatException("Please specify a parent view. Allowed format is ' parentViewKey [ # instanceID ] / viewKey [ # instanceID ]'");
            }
        }

        #endregion
    }
}