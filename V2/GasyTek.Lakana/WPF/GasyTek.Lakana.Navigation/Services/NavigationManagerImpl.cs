using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Adapters;
using GasyTek.Lakana.Navigation.Controls;

namespace GasyTek.Lakana.Navigation.Services
{
    internal class NavigationManagerImpl : INavigationManager
    {
        #region Fields

        private readonly IViewLocator _viewLocator;
        private readonly ViewStackCollectionManager _viewStackCollectionManager;
        private IWorkspaceAdapter _workspaceAdapter;

        #endregion

        #region Properties

        internal ViewStackCollectionManager ViewStackCollectionManager
        {
            get { return _viewStackCollectionManager; }
        }

        public ViewInfo ActiveView
        {
            get
            {
                var activeNode = ActiveNode;
                return activeNode != null ? activeNode.Value : ViewInfo.Null;
            }
        }

        internal LinkedListNode<ViewInfo> ActiveNode
        {
            get { return _viewStackCollectionManager.GetActiveNode(); }
        }

        internal int NbViews
        {
            get { return _viewStackCollectionManager.NbViews; }
        }

        #endregion

        #region Constructor

        public NavigationManagerImpl(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
            _viewStackCollectionManager = new ViewStackCollectionManager();
        }

        #endregion

        #region Public methods

        public ViewInfo NavigateTo(string navigationKey, object viewModel)
        {
            return NavigateToInternal(navigationKey, viewModel, false, false);
        }

        public ViewInfo NavigateTo(string navigationKey)
        {
            return NavigateTo(navigationKey, null);
        }

        public ModalResult<TResult> ShowModal<TResult>(string navigationKey, object viewModel)
        {
            NavigationKey.EnsuresNavigationKeyHasParent(navigationKey);

            var viewInfo = NavigateToInternal(navigationKey, viewModel, true, false);
            var modalHostControl = (ModalHostControl)viewInfo.InternalViewInstance;

            return new ModalResult<TResult>(modalHostControl.ResultCompletionSource.Task) { ViewInfo = viewInfo };
        }

        public ModalResult<TResult> ShowModal<TResult>(string navigationKey)
        {
            return ShowModal<TResult>(navigationKey, null);
        }

        public Task<MessageBoxResult> ShowMessageBox(string ownerViewKey, string message = "", MessageBoxImage messageBoxImage = MessageBoxImage.Information, MessageBoxButton messageBoxButton = MessageBoxButton.OK)
        {
            var modalViewInstanceKey = Guid.NewGuid().ToString("N");
            var navigationKey = string.Format("{0}/{1}", ownerViewKey, modalViewInstanceKey);
            var viewInfo = NavigateToInternal(navigationKey, null, true, true);

            // initialize messagebox informations
            var messageBoxControl = (MessageBoxControl)viewInfo.ViewInstance;
            messageBoxControl.ViewInstanceKey = modalViewInstanceKey;
            messageBoxControl.NavigationManager = this;
            messageBoxControl.Message = message;
            messageBoxControl.MessageBoxImage = messageBoxImage;
            messageBoxControl.MessageBoxButton = messageBoxButton;

            var modalHostControl = (ModalHostControl)viewInfo.InternalViewInstance;
            var modalResult = new ModalResult<MessageBoxResult>(modalHostControl.ResultCompletionSource.Task);

            return modalResult.AsyncResult;
        }

        public ViewInfo Close(string viewKey, object modalResult = null)
        {
            if (!_viewStackCollectionManager.IsTopMostView(viewKey))
                throw new CannotCloseNotTopMostViewException(viewKey);

            var closedNode = _viewStackCollectionManager.RemoveViewNode(viewKey);

            if (closedNode.Value.IsModal)
            {
                var modalHostControl = (ModalHostControl)closedNode.Value.InternalViewInstance;
                modalHostControl.ResultCompletionSource.SetResult(modalResult);
            }

            // perform update of the UI
            var newNode = _viewStackCollectionManager.GetActiveNode();
            var oldNode = closedNode;
            _workspaceAdapter.PerformClose(newNode, oldNode);

            return closedNode.Value;
        }

        public bool CloseApplication(bool forceClose = false)
        {
            // exit the application without any check
            if (forceClose)
            {
                OnCloseApplicationExecuted();
                return true;
            }

            // if there is no views or view models that prevent the app from closing, then close
            var notCloseableViews = _viewStackCollectionManager.GetNotCloseableViews().ToList();
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
                Views = new ObservableCollection<ViewInfo>(notCloseableViews.OrderBy(v => v.ViewInstanceKey).ToList()),
                NavigationManager = this
            };

            var dialogResult = shutdownApplicationWindow.ShowDialog();
            return dialogResult != null && dialogResult.Value;
        }

        private ViewInfo NavigateToInternal(string navigationKey, object viewModel, bool isModal, bool isMessageBox)
        {
            var navigationKeyInstance = NavigationKey.Parse(navigationKey);
            var parentViewInstanceKey = navigationKeyInstance.ParentViewInstanceKey;
            var viewInstanceKey = navigationKeyInstance.ViewInstanceKey;
            var viewKey = navigationKeyInstance.ViewKey;
            var hasParent = navigationKeyInstance.HasParent;
            var oldNode = _viewStackCollectionManager.GetActiveNode();

            LinkedListNode<ViewInfo> newNode;

            // if the navigation key has a complex form e.g : "parentView/view"

            if (hasParent)
            {
                if (_viewStackCollectionManager.TryFindViewNode(viewInstanceKey, out newNode))
                {
                    // activates an existing node
                    EnsuresViewHasParentAndMatch(newNode, parentViewInstanceKey);
                    _viewStackCollectionManager.ActivateExistingNode(newNode);
                }
                else
                {
                    // activates a new node
                    EnsureParentViewExist(parentViewInstanceKey);
                    EnsureParentViewIsTopMost(parentViewInstanceKey);

                    newNode = CreateNewNodeFrom(viewInstanceKey, viewKey, viewModel, isModal, isMessageBox);

                    var parentNode = _viewStackCollectionManager.FindViewNode(parentViewInstanceKey);
                    _viewStackCollectionManager.ActivateNewNode(newNode, (ViewStack)parentNode.List);
                }

                // perform update of the UI
                _workspaceAdapter.PerformActivation(newNode, oldNode);

                return newNode.Value;
            }

            // if the navigation key has a simple form e.g : "view"

            if (_viewStackCollectionManager.TryFindViewNode(viewInstanceKey, out newNode))
            {
                // activates an existing node
                newNode = _viewStackCollectionManager.IsTopMostView(viewInstanceKey) ? newNode : newNode.List.Last;
                _viewStackCollectionManager.ActivateExistingNode(newNode);
            }
            else
            {
                // activates a new node
                newNode = CreateNewNodeFrom(viewInstanceKey, viewKey, viewModel, isModal, isMessageBox);
                _viewStackCollectionManager.ActivateNewNode(newNode);
            }

            // perform update of the UI
            _workspaceAdapter.PerformActivation(newNode, oldNode);

            return newNode.Value;
        }

        #endregion

        #region Internal methods

        internal void SetMainWorkspace(Panel mainWorkspace, IWorkspaceAdapter workspaceAdapter)
        {
            if (workspaceAdapter == null)
                throw new ArgumentNullException("workspaceAdapter");

            _workspaceAdapter = workspaceAdapter;
            _workspaceAdapter.SetMainWorkspace(mainWorkspace);
            _workspaceAdapter.SetViewStackCollection(_viewStackCollectionManager.ViewStackCollection);

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

        private LinkedListNode<ViewInfo> CreateNewNodeFrom(string viewInstanceKey, string viewKey, object viewModel, bool isModal, bool isMessageBox)
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
                modalHost.ModalContent = viewInstance;
                internalViewInstance = modalHost;
            }
           
            var viewInfo = new ViewInfo(viewInstanceKey)
                       {
                           UIMetadata = GetUIMetadata(viewInstance, viewModel),
                           InternalViewInstance = internalViewInstance,
                           ViewInstance = viewInstance,
                           ViewModelInstance = viewModel,
                           IsModal = isModal,
                           IsMessageBox = isMessageBox
                       };

            return new LinkedListNode<ViewInfo>(viewInfo);
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

        private void EnsuresViewHasParentAndMatch(LinkedListNode<ViewInfo> node, string expectedParentViewInstanceKey)
        {
            var expectedParentView = new ViewInfo(expectedParentViewInstanceKey);
            if (node.Previous == null || (node.Previous != null && node.Previous.Value != expectedParentView))
            {
                throw new OnlyNewViewInstanceCanBeStackedException(node.Value.ViewInstanceKey, expectedParentViewInstanceKey);
            }
        }

        private void EnsureParentViewIsTopMost(string parentViewInstanceKey)
        {
            if (_viewStackCollectionManager.IsTopMostView(parentViewInstanceKey) == false)
                throw new ParentViewInstanceNotTopMostException(parentViewInstanceKey);
        }

        private void EnsureParentViewExist(string parentViewInstanceKey)
        {
            if (_viewStackCollectionManager.ContainsViewNode(parentViewInstanceKey) == false)
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

        #region Private class

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
            internal string ParentViewKey { get; private set; }

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

                throw new NavigationKeyFormatException("Please specify a parent. Allowed format is ' parentViewKey [ # instanceID ] / viewKey [ # instanceID ]'");
            }
        }

        #endregion


    }
}