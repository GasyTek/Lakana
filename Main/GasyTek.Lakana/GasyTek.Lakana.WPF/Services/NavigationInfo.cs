namespace GasyTek.Lakana.WPF.Services
{
    /// <summary>
    /// Serves as parameter for the methods of <see cref="INavigationService"/>
    /// </summary>
    public struct NavigationInfo
    {
        public string ViewKey { get; private set; }

        public string ParentViewKey { get; private set; }

        public bool HasParentKey
        {
            get { return !string.IsNullOrEmpty(ParentViewKey); }
        }

        /// <summary>
        /// Specify the object that will be used as DataContext for the view.
        /// It will contain the view model in a MVVM scenario.
        /// </summary>
        public object ViewModel { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the view will appear in <see cref="INavigationService.OpenedViews"/> or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the view will appear iin <see cref="INavigationService.OpenedViews"/>; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpenedViewMember { get; private set; }

        private NavigationInfo(string viewKey, string parentViewKey, object viewModel, bool isOpenedViewMember)
            : this()
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

        public static NavigationInfo CreateSimple(string viewKey, object viewContext, bool isOpenedView = true)
        {
            return new NavigationInfo(viewKey, null, viewContext, isOpenedView);
        }

        public static NavigationInfo CreateComplex(string viewKey, string parentViewKey, bool isOpenedView = true)
        {
            return new NavigationInfo(viewKey, parentViewKey, null, isOpenedView);
        }

        public static NavigationInfo CreateComplex(string viewKey, string parentViewKey, object viewContext, bool isOpenedView = true)
        {
            return new NavigationInfo(viewKey, parentViewKey, viewContext, isOpenedView);
        }

        #endregion
    }
}
