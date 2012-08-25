using System;

namespace GasyTek.Lakana.Navigation.Services
{
    /// <summary>
    /// Thrown when view is not found.
    /// </summary>
    public class ViewNotFoundException : ApplicationException
    {
        public ViewNotFoundException()
        {
            
        }

        public ViewNotFoundException(string viewkey) 
            : base("View with key " + viewkey + " was not found")
        {
            
        }
    }

    /// <summary>
    /// Thrown when the parent view on which we want to add child is not the top most on the stack of views.
    /// </summary>
    public class ParentViewNotTopMostException : ApplicationException
    {
        public ParentViewNotTopMostException(string viewkey) 
            : base("Parent view with key " + viewkey + " is not top most")
        {
            
        }
    }

    /// <summary>
    /// Thrown when the user tries to link dynamically existing view instance to an existing parent.
    /// Only new view are allowed to be linked with a parent.
    /// </summary>
    public class ChildViewAlreadyExistsException : ApplicationException
    {
        public ChildViewAlreadyExistsException(string viewkey, string parentViewKey)
            : base("The view with key " + viewkey + " can't be attached to parent view with key " + parentViewKey + " because the view already exists and is not modifiable")
        {
            
        }
    }
    
}
