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

        #endregion

        #region Constructor

        static NavigationManager()
        {
            NavigationManagerImpl = new NavigationManagerImpl(new ViewLocator());
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
                    NavigationManagerImpl.SetMainWorkspace(mainWorkspace, workspaceAdapter);
                }

                // TODO : implement a better mechanism to assign the workspace adapter according to the grid's type
            }
        }

        #endregion

        #region Public api

        public static ViewInfo ActiveView
        {
            get { return NavigationManagerImpl.ActiveView; }
        }

        //public static ReadOnlyObservableCollection<ViewInfo> OpenedViews
        //{
        //    get { return NavigationManagerImpl.OpenedViews; }
        //}

        //public static int NbOpenedViews
        //{
        //    get { return NavigationManagerImpl.NbTotalViews; }
        //}

        //public static void ChangeTransitionAnimation(TransitionAnimationProvider transitionAnimationProvider)
        //{
        //    NavigationManagerImpl.ChangeTransitionAnimation(transitionAnimationProvider);
        //}

        public static ViewInfo NavigateTo(string navigationKey)
        {
            return NavigationManagerImpl.NavigateTo(navigationKey);
        }

        public static ViewInfo NavigateTo(string navigationKey, object viewModel)
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

        public static ViewInfo Close(string viewKey, object modalResult = null)
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
