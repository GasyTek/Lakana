using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace GasyTek.Lakana.Navigation.Services
{
    public class ViewGroupCollectionManager
    {
        private readonly ViewGroupCollection _viewGroupCollection;
        private readonly ObservableCollection<View> _viewCollection;
        private readonly ReadOnlyObservableCollection<View> _readOnlyViewCollection;

        #region Properties

        public ReadOnlyObservableCollection<View> ViewCollection
        {
            get { return _readOnlyViewCollection; }
        }

        public int NbViews
        {
            get { return _viewGroupCollection.Sum(v => v.Count); }
        }

        public ViewGroupCollection ViewGroupCollection
        {
            get { return _viewGroupCollection; }
        }

        #endregion

        #region Constructor

        internal ViewGroupCollectionManager()
        {
            _viewGroupCollection = new ViewGroupCollection();
            _viewCollection = new ObservableCollection<View>();
            _readOnlyViewCollection = new ReadOnlyObservableCollection<View>(_viewCollection);
        }

        #endregion

        #region Public methods

        public View GetActiveView()
        {
            var activeNode = GetActiveNode();
            return activeNode != null ? activeNode.Value : View.Null;
        }

        public LinkedListNode<View> GetActiveNode()
        {
            var activeViewGroup = GetActiveViewGroup();
            return activeViewGroup != null ? activeViewGroup.Last : null;
        }

        public LinkedList<View> GetActiveViewGroup()
        {
            if (!_viewGroupCollection.Any()) return null;
            if (!_viewGroupCollection.Last.Value.Any()) return null;
            return _viewGroupCollection.Last.Value;
        }

        #endregion

        #region Internal methods

        internal void ActivateExistingNode(LinkedListNode<View> node)
        {
            if (GetActiveView() != node.Value)
            {
                _viewGroupCollection.Remove((ViewGroup)node.List);
                _viewGroupCollection.AddLast((ViewGroup)node.List);
            }
        }

        internal void ActivateNewNode(LinkedListNode<View> newNode, ViewGroup ownerGroup = null)
        {
            if (ownerGroup != null)
                ownerGroup.AddLast(newNode);
            else
            {
                var newStack = new ViewGroup();
                newStack.AddLast(newNode);
                _viewGroupCollection.AddLast(newStack);
            }

            if (!newNode.Value.IsMessageBox)
                _viewCollection.Add(newNode.Value);
        }

        internal LinkedListNode<View> FindViewNode(string viewInstanceKey)
        {
            LinkedListNode<View> node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                return node;
            }
            throw new ViewInstanceNotFoundException(viewInstanceKey);
        }

        internal bool ContainsViewNode(string viewInstanceKey)
        {
            LinkedListNode<View> node;
            return TryFindViewNode(viewInstanceKey, out node);
        }

        internal bool TryFindViewNode(string viewInstanceKey, out LinkedListNode<View> node)
        {
            var viewInfo = new View(viewInstanceKey);
            var q = (from stack in _viewGroupCollection
                     from v in stack
                     where v == viewInfo
                     select stack.Find(v)).ToList();
            node = q.FirstOrDefault();
            return q.Any();
        }

        internal ClosedNode RemoveViewNode(string viewInstanceKey)
        {
            LinkedListNode<View> node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                var viewGroup = (ViewGroup)node.List;
                viewGroup.Remove(node);

                if (viewGroup.Count == 0)
                    _viewGroupCollection.Remove(viewGroup);

                _viewCollection.Remove(node.Value);

                return new ClosedNode { View = node.Value, ViewGroup = viewGroup };
            }

            return new ClosedNode { View = node.Value };
        }

        internal bool IsTopMostView(string viewInstanceKey)
        {
            LinkedListNode<View> node;
            if (TryFindViewNode(viewInstanceKey, out node))
            {
                return node.List.Last.Value == node.Value;
            }
            return false;
        }

        internal IEnumerable<View> GetNotCloseableViews()
        {
            // retrieve views that are parent of top most message box views
            var q1 = (from vs in _viewGroupCollection
                      let last = vs.Last
                      let lastPrevious = vs.Last.Previous
                      where last != null
                         && lastPrevious != null
                         && last.Value.IsMessageBox
                      select lastPrevious.Value).ToList();

            // retrieve top most views except message boxes
            var q2 = (from vs in _viewGroupCollection
                      let last = vs.Last
                      where last != null
                      select last.Value).Except(q1);

            // retrieve views/viewmodels that are not closeable
            return from vi in q1.Concat(q2)
                   let v = vi.ViewInstance as ICloseable
                   let vm = vi.ViewInstance.DataContext as ICloseable
                   where (v != null && v.CanClose() == false) || (vm != null && vm.CanClose() == false)
                   select vi;
        }


        #endregion
    }

    public class ViewGroup : LinkedList<View>
    {
        public Stack<FrameworkElement> ToStack()
        {
            return new Stack<FrameworkElement>(this.Select(vi => vi.InternalViewInstance));
        }
    }

    public class ViewGroupCollection : LinkedList<ViewGroup>
    {

    }

    public class ClosedNode
    {
        public View View { get; internal set; }
        public ViewGroup ViewGroup { get; internal set; }
    }
}
