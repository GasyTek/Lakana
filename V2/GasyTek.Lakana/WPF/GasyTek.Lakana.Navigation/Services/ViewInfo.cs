using System;
using System.Windows;
using GasyTek.Lakana.Common.UI;

namespace GasyTek.Lakana.Navigation.Services
{
    public struct ViewInfo : IEquatable<ViewInfo>
    {
        /// <summary>
        /// Gets the view key.
        /// </summary>
        /// <value>
        /// The view key.
        /// </value>
        public string ViewInstanceKey { get; internal set; }

        /// <summary>
        /// Gets the actual view instance.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public FrameworkElement ViewInstance { get; internal set; }

        /// <summary>
        /// Used by the infrastructure. 
        /// It can contain either the actual or the container view (for modal views for example). 
        /// </summary>
        internal FrameworkElement InternalViewInstance { get; set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public object ViewModelInstance { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the view is modal or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is modal; otherwise, <c>false</c>.
        /// </value>
        public bool IsModal { get; internal set; }

        /// <summary>
        /// Used by the infrastructure to determine if this view is a message box or not
        /// </summary>
        internal bool IsMessageBox { get; set; }

        /// <summary>
        /// Gets the presentation metadatas associated with the view.
        /// </summary>
        /// <value>
        /// The presentation metadata.
        /// </value>
        public IUIMetadata UIMetadata { get; internal set; }

        internal ViewInfo(string viewInstanceKey)
            : this()
        {
            ViewInstanceKey = viewInstanceKey;
        }

        public static ViewInfo Null
        {
            get { return default(ViewInfo); }
        }

        public bool IsNull()
        {
            return this == Null;
        }

        #region Override Equals

        public bool Equals(ViewInfo other)
        {
            return Equals(other.ViewInstanceKey, ViewInstanceKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(ViewInfo)) return false;
            return Equals((ViewInfo)obj);
        }

        public override int GetHashCode()
        {
            return (ViewInstanceKey != null ? ViewInstanceKey.GetHashCode() : 0);
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