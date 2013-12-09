using System;
using System.Windows;
using GasyTek.Lakana.Common.UI;
using GasyTek.Lakana.Navigation.Controls;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Represents a view.
    /// It contains all informations attached to an actual view.
    /// </summary>
    public struct View : IEquatable<View>
    {
        /// <summary>
        /// Gets the view instance key (different than the view key).
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
        /// Contains the instance of the view host control that hosts the actual view.. 
        /// </summary>
        internal ViewHostControl ViewHostInstance { get; set; }

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

        internal View(string viewInstanceKey)
            : this()
        {
            ViewInstanceKey = viewInstanceKey;
        }

        public static View Null
        {
            get { return default(View); }
        }

        public bool IsNull()
        {
            return this == Null;
        }

        #region Override Equals

        public bool Equals(View other)
        {
            return Equals(other.ViewInstanceKey, ViewInstanceKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(View)) return false;
            return Equals((View)obj);
        }

        public override int GetHashCode()
        {
            return (ViewInstanceKey != null ? ViewInstanceKey.GetHashCode() : 0);
        }

        public static bool operator ==(View left, View right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(View left, View right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}