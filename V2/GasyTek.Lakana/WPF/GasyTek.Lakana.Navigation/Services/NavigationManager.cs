using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GasyTek.Lakana.Navigation.Adapters;

namespace GasyTek.Lakana.Navigation.Services
{
    public class NavigationManager : DependencyObject
    {
        #region Attached properties

        public static readonly DependencyProperty IsMainWorkspaceProperty =
            DependencyProperty.RegisterAttached("IsMainWorkspace", typeof(bool), typeof(NavigationManager), new PropertyMetadata(default(bool), OnIsMainWorkspaceChanged));

        public static void SetIsMainWorkspace(Panel element, bool value)
        {
            element.SetValue(IsMainWorkspaceProperty, value);
        }

        public static bool GetIsMainWorkspace(Panel element)
        {
            return (bool)element.GetValue(IsMainWorkspaceProperty);
        }

        #endregion

        #region Fields

        private static readonly NavigationManagerImpl NavigationManagerImpl;
        private static TransitionAnimation _transitionAnimation;

        #endregion

        #region Constructor

        static NavigationManager()
        {
            NavigationManagerImpl = new NavigationManagerImpl(new ViewLocator());
            _transitionAnimation = new TransitionAnimation();
        }

        #endregion

        #region Dependency property changed handler

        private static void OnIsMainWorkspaceChanged(DependencyObject owner, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var mainWorkspace = (Panel) owner;

                // the workspace is a Grid control
                if (mainWorkspace is Grid)
                {
                    var workspaceAdapter = new GridWorkspaceAdapter();
                    NavigationManagerImpl.SetMainWorkspace(mainWorkspace, workspaceAdapter, () => _transitionAnimation);
                }

                // TODO : implement a better mechanism to assign the workspace adapter according to the panel's concrete type
            }
        }

        #endregion
        
        #region Public api

        public static View ActiveView
        {
            get { return NavigationManagerImpl.ActiveView; }
        }

        public static ReadOnlyObservableCollection<View> Views
        {
            get
            {
                return NavigationManagerImpl.ViewGroupCollectionManager.ViewCollection;
            }
        }

        public static int NbViews
        {
            get { return NavigationManagerImpl.NbViews; }
        }

        public static void ChangeTransitionAnimation(TransitionAnimation transitionAnimation)
        {
            _transitionAnimation = transitionAnimation;
        }

        public static View NavigateTo(string navigationKey)
        {
            return NavigationManagerImpl.NavigateTo(navigationKey);
        }

        public static View NavigateTo(string navigationKey, object viewModel)
        {
            return NavigationManagerImpl.NavigateTo(navigationKey, viewModel);
        }

        public static ModalResult<TResult> ShowModal<TResult>(string navigationKey)
        {
            return NavigationManagerImpl.ShowModal<TResult>(navigationKey);
        }

        public static ModalResult<TResult> ShowModal<TResult>(string navigationKey, object viewModel)
        {
            return NavigationManagerImpl.ShowModal<TResult>(navigationKey, viewModel);
        }

        public static Task<MessageBoxResult> ShowMessageBox(string ownerViewKey, string message = "", MessageBoxImage messageBoxImage = MessageBoxImage.Asterisk, MessageBoxButton messageBoxButton = MessageBoxButton.OK)
        {
            return NavigationManagerImpl.ShowMessageBox(ownerViewKey, message, messageBoxImage, messageBoxButton);
        }

        public static View Close(string viewKey, object modalResult = null)
        {
            return NavigationManagerImpl.Close(viewKey, modalResult);
        }

        public static bool CloseApplication(bool forceClose = false)
        {
            return NavigationManagerImpl.CloseApplication(forceClose);
        }

        #endregion
    }
}
