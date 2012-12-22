namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Object that contains all information needed for navigation.
    /// </summary>
    public class NavigationInfo
    {
        public string ViewKey { get; private set; }

        public string ParentViewKey { get; private set; }

        public bool HasParent
        {
            get { return !string.IsNullOrEmpty(ParentViewKey); }
        }

        /// <summary>
        /// Specify the object that will be used as DataContext for the view.
        /// It will contain the view model in a MVVM scenario.
        /// </summary>
        public object ViewModel { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the view will appear in <see cref="INavigationManager2.OpenedViews"/> or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the view will appear iin <see cref="INavigationManager2.OpenedViews"/>; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpenedViewMember { get; private set; }

        protected NavigationInfo(string viewKey, string parentViewKey, object viewModel, bool isOpenedViewMember)
        {
            ViewKey = viewKey;
            ParentViewKey = parentViewKey;
            ViewModel = viewModel;
            IsOpenedViewMember = isOpenedViewMember;
        }

        #region Factory methods

        public static NavigationInfo CreateSimple(string viewKey, bool isOpenedView = true)
        {
            return new NavigationInfo(viewKey, null, null, isOpenedView);
        }

        public static NavigationInfo CreateSimple(string viewKey, object viewModel, bool isOpenedView = true)
        {
            return new NavigationInfo(viewKey, null, viewModel, isOpenedView);
        }

        public static NavigationInfo CreateComplex(string viewKey, string parentViewKey, bool isOpenedView = true)
        {
            return new NavigationInfo(viewKey, parentViewKey, null, isOpenedView);
        }

        public static NavigationInfo CreateComplex(string viewKey, string parentViewKey, object viewModel, bool isOpenedView = true)
        {
            return new NavigationInfo(viewKey, parentViewKey, viewModel, isOpenedView);
        }

        #endregion
    }

    /// <summary>
    /// A navigation info specialized for modal views.
    /// </summary>
    public class ModalNavigationInfo : NavigationInfo
    {
        private ModalNavigationInfo(string viewKey, string parentViewKey, object viewModel, bool isOpenedViewMember)
            : base(viewKey, parentViewKey, viewModel, isOpenedViewMember)
        {
        }

        public static ModalNavigationInfo Create(string viewKey, string parentViewKey, object viewModel = null, bool isOpenedView = true)
        {
            return new ModalNavigationInfo(viewKey, parentViewKey, viewModel, isOpenedView);
        }
    }
}
