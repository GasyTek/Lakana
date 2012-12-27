using System;
using System.Collections.Generic;
using GasyTek.Lakana.Navigation.Attributes;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Thrown when view instance is not found.
    /// </summary>
    public class ViewInstanceNotFoundException : ApplicationException
    {
        public ViewInstanceNotFoundException()
        {
        }

        public ViewInstanceNotFoundException(string viewkey) 
            : base("Instance with key " + viewkey + " was not found")
        {
        }
    }

    /// <summary>
    /// Thrown when parent view instance is not found.
    /// </summary>
    public class ParentViewInstanceNotFoundException : ViewInstanceNotFoundException
    {
        public ParentViewInstanceNotFoundException(string viewkey)
            : base(viewkey)
        {
        }
    }

    /// <summary>
    /// Thrown when the parent view on which we want to add child is not the top most on the stack of views.
    /// </summary>
    public class ParentViewInstanceNotTopMostException : ApplicationException
    {
        public ParentViewInstanceNotTopMostException(string viewkey) 
            : base("Parent view with key " + viewkey + " is not top most")
        {
        }
    }

    /// <summary>
    /// Thrown when the user tries to stack existing view instance onto another view.
    /// Only new view are allowed to be linked with a parent.
    /// </summary>
    public class OnlyNewViewInstanceCanBeStackedException : ApplicationException
    {
        public OnlyNewViewInstanceCanBeStackedException(string viewkey, string parentViewKey)
            : base("The view with key " + viewkey + " can't be attached to parent view with key " + parentViewKey + " because the view already exists and is not modifiable")
        {
        }
    }

    /// <summary>
    /// Thrown when the specified view key was not found.
    /// </summary>
    /// <remarks>Views are identified by view key that is used by the <see cref="IViewLocator"/> to instantiate it.</remarks>
    public class ViewKeyNotFoundException : ApplicationException
    {
        public ViewKeyNotFoundException(string viewKey)
            : base("Instance with key " + viewKey + " was not found, please use " + typeof(ViewKeyAttribute).Name + " to associate view unique key to views.")
        {
        }
    }

    /// <summary>
    /// Thrown when there are duplicate view keys that were regitered by the <see cref="IViewLocator"/>
    /// </summary>
    public class ViewKeyDuplicationException : ApplicationException
    {
        public ViewKeyDuplicationException(string viewKey, IEnumerable<string> viewTypeNames)
            : base(string.Format("Instance with key '" + viewKey + "' is duplicated. It is defined on types '{0}'", string.Join("\n", viewTypeNames)))
        {

        }
    }

    /// <summary>
    /// Thrown when there are duplicate view keys that were regitered by the <see cref="IViewLocator"/>
    /// </summary>
    public class ViewKeyNullOrEmptyException : ApplicationException
    {
        public ViewKeyNullOrEmptyException(string viewTypeName)
            : base(string.Format("Instance key must be initialized for view of type '{0}'.", viewTypeName))
        {
        }
    }

    /// <summary>
    /// Thrown when a navigation is in bad format.
    /// </summary>
    public class NavigationKeyFormatException : ApplicationException
    {
        public NavigationKeyFormatException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Thrown when attempting to close a view that is not top most.
    /// </summary>
    public class CannotCloseNotTopMostViewException : ApplicationException
    {
        public CannotCloseNotTopMostViewException(string viewKey)
            : base(viewKey)
        {
        }
    }
}
