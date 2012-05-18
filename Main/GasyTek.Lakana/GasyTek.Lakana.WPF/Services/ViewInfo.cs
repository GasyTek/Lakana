using System;
using System.Windows;
using GasyTek.Lakana.WPF.Common;

namespace GasyTek.Lakana.WPF.Services
{
    public struct ViewInfo : IEquatable<ViewInfo>
    {
        /// <summary>
        /// Getsthe view key.
        /// </summary>
        /// <value>
        /// The view key.
        /// </value>
        public string ViewKey { get; internal set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public FrameworkElement View { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is modal or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is modal; otherwise, <c>false</c>.
        /// </value>
        public bool IsModal { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the view will appear in <see cref="INavigationService.OpenedViews"/> or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the view will appear iin <see cref="INavigationService.OpenedViews"/>; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpenedViewMember { get; internal set; }

        /// <summary>
        /// Gets or sets the presentation metadatas associated with the view.
        /// </summary>
        /// <value>
        /// The presentation metadata.
        /// </value>
        public IUIMetadata UIMetadata { get; internal set; }

        internal ViewInfo(string viewKey)
            : this()
        {
            ViewKey = viewKey;
        }

        public static ViewInfo Null
        {
            get { return default(ViewInfo); }
        }

        #region Override Equals

        public bool Equals(ViewInfo other)
        {
            return Equals(other.ViewKey, ViewKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(ViewInfo)) return false;
            return Equals((ViewInfo)obj);
        }

        public override int GetHashCode()
        {
            return (ViewKey != null ? ViewKey.GetHashCode() : 0);
        }

        public static bool operator ==(ViewInfo left, ViewInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ViewInfo left, ViewInfo right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}